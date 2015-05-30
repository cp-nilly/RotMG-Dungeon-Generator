﻿/*
    Copyright (C) 2015 creepylava

    This file is part of RotMG Dungeon Generator.

    RotMG Dungeon Generator is free software: you can redistribute it and/or modify
    it under the terms of the GNU Affero General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Affero General Public License for more details.

    You should have received a copy of the GNU Affero General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.

*/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;
using DungeonGenerator.Dungeon;
using DungeonGenerator.Templates.PirateCave;

namespace DungeonGenerator {
	public partial class frmMain : Form {
		public frmMain() {
			InitializeComponent();
		}

		readonly Random rand = new Random();
		readonly List<Button> btns = new List<Button>();
		int seed;
		Generator gen;
		Rasterizer ras;

		void frmMain_Load(object sender, EventArgs e) {
			foreach (var value in Enum.GetValues(typeof(GenerationStep))) {
				if ((GenerationStep)value == GenerationStep.Finish)
					continue;
				var btn = new Button { Text = value.ToString(), Tag = value, AutoSize = true };
				btn.Click += Step_Click;
				stepsPane.Controls.Add(btn);
				btns.Add(btn);
			}
			foreach (var value in Enum.GetValues(typeof(RasterizationStep))) {
				if ((RasterizationStep)value == RasterizationStep.Finish)
					continue;
				var btn = new Button { Text = value.ToString(), Tag = value, AutoSize = true };
				btn.Click += Step_Click;
				stepsPane.Controls.Add(btn);
				btns.Add(btn);
			}
			stepsPane.Enabled = false;
		}

		void Step_Click(object sender, EventArgs e) {
			if (((Button)sender).Tag is GenerationStep) {
				var step = (GenerationStep)((Button)sender).Tag;
				gen.Generate(step + 1);
			}
			else {
				gen.Generate();
				if (ras == null)
					ras = new Rasterizer(seed, gen.ExportGraph());

				var step = (RasterizationStep)((Button)sender).Tag;
				ras.Rasterize(step + 1);
			}

			Render();
			foreach (var btn in btns) {
				if (btn.Tag is GenerationStep)
					btn.Enabled = (GenerationStep)btn.Tag >= gen.Step;
				else
					btn.Enabled = ras == null || (RasterizationStep)btn.Tag >= ras.Step;
			}
		}

		void Render() {
			if (cbBorder.Checked) {
				RenderBorder();
				return;
			}
			if (ras != null) {
				RenderRaster();
				return;
			}

			var rms = gen.GetRooms().ToList();
			int dx = int.MaxValue, dy = int.MaxValue;
			int mx = int.MinValue, my = int.MinValue;
			foreach (var rm in rms) {
				var bounds = rm.Bounds;

				if (bounds.X < dx)
					dx = bounds.X;
				if (bounds.Y < dy)
					dy = bounds.Y;

				if (bounds.MaxX > mx)
					mx = bounds.MaxX;
				if (bounds.MaxY > my)
					my = bounds.MaxY;
			}

			const int Factor = 4;

			var bmp = new Bitmap((mx - dx + 4) * Factor, (my - dy + 4) * Factor);
			using (var g = Graphics.FromImage(bmp)) {
				g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
				foreach (var rm in rms) {
					var brush = Brushes.Black;
					if (rm.Type == RoomType.Start)
						brush = Brushes.Red;
					else if (rm.Type == RoomType.Target)
						brush = Brushes.Green;
					else if (rm.Type == RoomType.Special)
						brush = Brushes.Blue;

					var x = (rm.Pos.X - dx) * Factor + 2 * Factor;
					var y = (rm.Pos.Y - dy) * Factor + 2 * Factor;
					g.FillRectangle(brush, x, y, rm.Width * Factor, rm.Height * Factor);
					g.DrawString(rm.Depth.ToString(), Font, Brushes.White, x, y);
				}
			}

			var original = box.Image;
			box.Image = bmp;
			if (original != null)
				original.Dispose();
		}

		void RenderBorder() {
			var rms = gen.GetRooms().ToList();
			int dx = int.MaxValue, dy = int.MaxValue;
			int mx = int.MinValue, my = int.MinValue;
			foreach (var rm in rms) {
				var bounds = rm.Bounds;

				if (bounds.X < dx)
					dx = bounds.X;
				if (bounds.Y < dy)
					dy = bounds.Y;

				if (bounds.MaxX > mx)
					mx = bounds.MaxX;
				if (bounds.MaxY > my)
					my = bounds.MaxY;
			}

			const int Factor = 4;

			var pen = new Pen(Color.Black, Factor / 2);
			var bmp = new Bitmap((mx - dx + 4) * Factor, (my - dy + 4) * Factor);
			using (var g = Graphics.FromImage(bmp)) {
				g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
				foreach (var rm in rms) {
					var rmPen = pen;
					if (rm.Type == RoomType.Start)
						rmPen = new Pen(Color.Red, Factor / 2);
					else if (rm.Type == RoomType.Target)
						rmPen = new Pen(Color.Green, Factor / 2);
					else if (rm.Type == RoomType.Special)
						rmPen = new Pen(Color.Blue, Factor / 2);

					var x = (rm.Pos.X - dx) * Factor + 2 * Factor;
					var y = (rm.Pos.Y - dy) * Factor + 2 * Factor;
					g.DrawRectangle(rmPen, x, y, rm.Width * Factor, rm.Height * Factor);
					g.DrawString(rm.Depth.ToString(), Font, Brushes.Black, x, y);

					if (rmPen != pen)
						rmPen.Dispose();
				}
			}

			pen.Dispose();

			var original = box.Image;
			box.Image = bmp;
			if (original != null)
				original.Dispose();
		}

		void RenderRaster() {
			var map = ras.ExportMap();
			var bmp = new Bitmap(map.GetUpperBound(0), map.GetUpperBound(1));

			for (int y = 0; y < bmp.Height; y++)
				for (int x = 0; x < bmp.Width; x++) {
					if (map[x, y].TileType != 0xfe)
						bmp.SetPixel(x, y, Color.Black);
				}

			var original = box.Image;
			box.Image = bmp;
			if (original != null)
				original.Dispose();
		}

		void btnNew_Click(object sender, EventArgs e) {
			btnNewStep_Click(sender, e);
			Step_Click(btns.Last(), e);
		}

		void btnNewStep_Click(object sender, EventArgs e) {
			seed = rand.Next();
			gen = new Generator(seed, new PirateCaveTemplate());
			ras = null;
			Text = ProductName + " [Seed: " + seed + "]";

			stepsPane.Enabled = true;
			foreach (var btn in btns)
				btn.Enabled = true;

			var original = box.Image;
			box.Image = null;
			if (original != null)
				original.Dispose();
		}

		void cbBorder_CheckedChanged(object sender, EventArgs e) {
			if (stepsPane.Enabled)
				Render();
		}
	}
}