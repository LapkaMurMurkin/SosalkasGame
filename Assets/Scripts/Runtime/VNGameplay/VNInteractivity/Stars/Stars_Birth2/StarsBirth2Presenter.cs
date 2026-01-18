using System.Collections.Generic;
using System.Linq;
using Extensions;
using SosalkasGame.Runtime.Core.GameFSM;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;

namespace SosalkasGame.Runtime.VNGameplay.VNInteractivity.Stars.Stars_Birth2
{
    public class StarsBirth2Presenter : InteractivityPresenter
    {
        public Stack<Image> ResultPath { get; private set; }
        public Dictionary<Image, List<Image>> ResultPathConnections { get; private set; }

        public StarsBirth2Presenter(IObjectResolver resolver) : base(resolver)
        {
            ResultPath = new();
            ResultPathConnections = new();
        }

        public Dictionary<Image, List<Image>> BuildConnections(List<Image> stars, LineRenderer starPath)
        {
            Dictionary<Image, List<Image>> connections = new();
            Vector3[] pathPoints = new Vector3[starPath.positionCount];
            starPath.GetPositions(pathPoints);
            int count = pathPoints.Length;
            for (int i = 0; i < count; i++)
            {
                Image current = GetNearestStar(stars, pathPoints[i]);
                Image next = GetNearestStar(stars, pathPoints[(i + 1) % count]);
                Image prev = GetNearestStar(stars, pathPoints[(i - 1 + count) % count]);

                AddConnection(connections, current, next);
                AddConnection(connections, current, prev);
            }

            return connections;
        }

        private void AddConnection(Dictionary<Image, List<Image>> graph, Image from, Image to)
        {
            if (graph.ContainsKey(from) is false)
                graph[from] = new List<Image>();

            if (graph[from].Contains(to) is false)
                graph[from].Add(to);
        }

        private void RemoveConnection(Dictionary<Image, List<Image>> graph, Image from, Image to)
        {
            if (graph.ContainsKey(from))
            {
                graph[from].Remove(to);
                if (graph[from].IsNullOrEmpty())
                    graph.Remove(from);
            }
        }

        public Image GetNearestStar(List<Image> stars, Vector3 pathPoint)
        {
            Vector3[] starPositions = stars.Select(star => star.rectTransform.position).ToArray();
            VectorMath.FindNearest(pathPoint, starPositions, out int index);
            return stars[index];
        }

        public void AddStarToResultPath(Image newStar)
        {
            if (ResultPath.TryPeek(out Image lastStar) is false)
            {
                ResultPath.Push(newStar);
                return;
            }

            if (CheckIsConnectionExist(ResultPathConnections, lastStar, newStar))
                return;

            ResultPath.Push(newStar);
            AddConnection(ResultPathConnections, lastStar, newStar);
            AddConnection(ResultPathConnections, newStar, lastStar);
        }

        public void RemoveLastStarFromResultPath()
        {
            if (ResultPath.TryPop(out Image lastStar) is false)
                return;

            if (ResultPath.TryPeek(out Image penultimateStar) is false)
                return;

            RemoveConnection(ResultPathConnections, lastStar, penultimateStar);
            RemoveConnection(ResultPathConnections, penultimateStar, lastStar);
        }

        public bool CheckIsConnectionExist(Dictionary<Image, List<Image>> graph, Image from, Image to)
        {
            if (graph.ContainsKey(from) && graph[from].IsNullOrEmpty() is false && graph[from].Contains(to))
                return true;

            return false;
        }
    }
}