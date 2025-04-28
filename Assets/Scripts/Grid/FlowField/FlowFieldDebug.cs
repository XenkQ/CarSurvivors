using System;
using TMPro;
using UnityEngine;

namespace Grid.FlowField
{
    [Serializable]
    public class FlowFieldDebugConfiguration
    {
        public Grid grid;
        public Transform debugInfoHolder;
        public float fontSize;
        public float textYOffset;
        public FlowFieldDebug.DisplayMode displayMode;

        public FlowFieldDebugConfiguration(Grid grid,
                                           Transform debugInfoHolder,
                                           float fontSize = 1f,
                                           float textYOffset = 0,
                                           FlowFieldDebug.DisplayMode displayMode = FlowFieldDebug.DisplayMode.CostField)
        {
            this.grid = grid;
            this.debugInfoHolder = debugInfoHolder;
            this.fontSize = fontSize;
            this.textYOffset = textYOffset;
            this.displayMode = displayMode;
        }
    }

    public static class FlowFieldDebug
    {
        public enum DisplayMode
        {
            CostField,
            IntegrationField,
            FlowField
        }

        public static void DisplayFlowFieldDebugTextOnGrid(FlowFieldDebugConfiguration configuration)
        {
            bool cellDebugTextWasPreviouslyCreated = configuration.debugInfoHolder.childCount > 0;
            int currentChildIndex = 0;
            Cell[,] cells = configuration.grid.Cells;
            for (int i = 0; i < cells.GetLength(0); i++)
            {
                for (int j = 0; j < cells.GetLength(1); j++)
                {
                    Cell cell = cells[i, j];
                    byte cellCost = cell.Cost;
                    TextMeshPro textComponent;
                    if (cellDebugTextWasPreviouslyCreated)
                    {
                        textComponent = configuration.debugInfoHolder.GetChild(currentChildIndex).GetComponent<TextMeshPro>();
                        textComponent.text = GetCellDebugTextBasedOnMode(cell, configuration.displayMode);
                        currentChildIndex++;
                    }
                    else
                    {
                        textComponent = CreateCellDebugText(configuration, cell, cellCost.ToString());
                        textComponent.text = GetCellDebugTextBasedOnMode(cell, configuration.displayMode);
                    }
                }
            }
        }

        private static TextMeshPro CreateCellDebugText(FlowFieldDebugConfiguration configuration, Cell cell, string textObjectName)
        {
            GameObject visualizer = new(textObjectName);
            visualizer.transform.parent = configuration.debugInfoHolder;
            visualizer.transform.SetPositionAndRotation(new Vector3(cell.WorldPos.x, configuration.textYOffset, cell.WorldPos.z),
                                                        Quaternion.Euler(new Vector3(90, 0, 0)));
            var rectTransform = visualizer.AddComponent<RectTransform>();
            rectTransform.rect.Set(0, 0, configuration.grid.CellSize, configuration.grid.CellSize);
            visualizer.AddComponent<MeshRenderer>();
            var textComponent = visualizer.AddComponent<TextMeshPro>();
            textComponent.fontSize = configuration.fontSize;
            textComponent.alignment = TextAlignmentOptions.Center;
            return textComponent;
        }

        private static string GetCellDebugTextBasedOnMode(Cell cell, DisplayMode displayMode) => displayMode switch
        {
            DisplayMode.CostField => cell.Cost.ToString(),
            DisplayMode.IntegrationField => cell.BestCost.ToString(),
            DisplayMode.FlowField => cell.BestDirection.Vector.ToString(),
            _ => ""
        };
    }
}
