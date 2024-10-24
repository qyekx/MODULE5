using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ex1
{
    public partial class Form1 : Form
    {
        private string selectedShape = string.Empty; // Выбранная фигура
        private Point startPoint; // Начальная точка для рисования
        private bool isDrawing = false; // Флаг, который указывает, что идет процесс рисования
        private List<(string Shape, Point Start, Point End)> shapes = new List<(string, Point, Point)>(); // Список для хранения фигур

        public Form1()
        {
            InitializeComponent();
        }

        // Выбор фигуры для рисования
        private void btnLine_Click(object sender, EventArgs e) => SelectShape("Линия");

        private void btnCircle_Click(object sender, EventArgs e) => SelectShape("Круг");

        private void btnSquare_Click(object sender, EventArgs e) => SelectShape("Квадрат");

        private void SelectShape(string shape)
        {
            selectedShape = shape;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearShapes();
        }

        private void ClearShapes()
        {
            shapes.Clear();
            panelCanvas.Invalidate(); // Перерисовываем панель
        }

        // Обработка событий мыши для рисования
        private void panelCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (IsShapeSelected())
            {
                StartDrawing(e.Location);
            }
        }

        private void panelCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                FinishDrawing(e.Location);
            }
        }

        private bool IsShapeSelected() => !string.IsNullOrEmpty(selectedShape);

        private void StartDrawing(Point location)
        {
            startPoint = location; // Запоминаем начальную точку
            isDrawing = true; // Устанавливаем флаг начала рисования
        }

        private void FinishDrawing(Point endPoint)
        {
            isDrawing = false;
            AddShapeToList(endPoint); // Добавляем фигуру в список
            panelCanvas.Invalidate(); // Перерисовываем панель
        }

        private void AddShapeToList(Point endPoint)
        {
            shapes.Add((selectedShape, startPoint, endPoint)); // Добавляем фигуру с координатами
        }

        // Отрисовка фигур на панели
        private void panelCanvas_Paint(object sender, PaintEventArgs e)
        {
            DrawShapes(e.Graphics);
        }

        private void DrawShapes(Graphics g)
        {
            foreach (var shape in shapes)
            {
                switch (shape.Shape)
                {
                    case "Линия":
                        DrawLine(g, shape.Start, shape.End);
                        break;
                    case "Круг":
                        DrawCircle(g, shape.Start, shape.End);
                        break;
                    case "Квадрат":
                        DrawSquare(g, shape.Start, shape.End);
                        break;
                }
            }
        }

        private void DrawLine(Graphics g, Point start, Point end)
        {
            g.DrawLine(Pens.Black, start, end); // Рисуем линию
        }

        private void DrawCircle(Graphics g, Point start, Point end)
        {
            int radius = CalculateRadius(start, end);
            Point center = CalculateCenter(start, end, radius);
            g.DrawEllipse(Pens.Black, center.X, center.Y, radius * 2, radius * 2); // Рисуем круг
        }

        private void DrawSquare(Graphics g, Point start, Point end)
        {
            int side = CalculateSide(start, end);
            g.DrawRectangle(Pens.Black, start.X, start.Y, side, side); // Рисуем квадрат
        }

        // Вспомогательные методы вычислений
        private int CalculateRadius(Point start, Point end)
        {
            return Math.Min(Math.Abs(end.X - start.X), Math.Abs(end.Y - start.Y)) / 2;
        }

        private Point CalculateCenter(Point start, Point end, int radius)
        {
            int centerX = (start.X + end.X) / 2 - radius;
            int centerY = (start.Y + end.Y) / 2 - radius;
            return new Point(centerX, centerY);
        }

        private int CalculateSide(Point start, Point end)
        {
            return Math.Min(Math.Abs(end.X - start.X), Math.Abs(end.Y - start.Y));
        }
    }
}
