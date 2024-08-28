#region FileInfo

// <summary>
// Author  hanhinhe
// Date    2023.08.01
// Desc    左手坐标系,z朝下, 南是0,0  北是60，60 西是 0,60 东 60,0 
//         Mapcell getcell ( y , x ) 巨奇葩
// </summary>

#endregion

using System;
using System.Collections.Generic;
using HHL.Common;
using IGG.Game.Data.Cache;
using IGG.Game.Module.CityBuilding;
using IGG.Game.PathFinding;
using Unity.Mathematics;
using UnityEngine;

namespace HHL.Game
{
    public class CityMapTool : MonoBehaviour
    {
        public Map Map;
        private Building m_wall;
        public Vector3 CubeSize = Vector3.one * 0.01f;
        public Vector3 CubeOffSet = Vector3.zero;

        private void Start()
        {
            Map = AppCache.CityBuilding.MyCompCity.PathFindingMap;
            m_wall = AppCache.CityBuilding.MyCompCity.GetBuilding(EBuildingType.CityWall);
        }


        public void Update()
        {
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            for (var i = 0; i < Map.RowCount; i++)
            {
                for (var j = 0; j < Map.ColumnCount; j++)
                {
                    var mapCell = Map.GetCell(j, i);
                    var pos = CoordHelper.GridToCartesianCoord(new int2(i, j));
                    pos += m_wall.Owner.LocalCenterOffset + m_wall.Owner.Trans.position;
                    //pos.y = -0.02f;
                    if (mapCell.HasObstacle)
                    {
                        Gizmos.color = Color.red;
                    }
                    else
                    {
                        Gizmos.color = Color.green;
                    }

                    pos += CubeOffSet;

                    if (i == 0 && j == 0)
                    {
                        Gizmos.DrawSphere(pos, CubeSize.x);
                    }
                    else
                    {
                        Gizmos.DrawCube(pos, CubeSize);
                    }
                }
            }
        }
#endif
    }
}