#region FileInfo

// <summary>
// Author  hanhinhe
// Date    2023.08.01
// Desc
// </summary>

#endregion

using System.Collections.Generic;
using IGG.Framework.Cache;
using IGG.Framework.Extend;
using IGG.Framework.Load;
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
        public Vector3 StartPos;
        public List<List<GameObject>> CellList = new List<List<GameObject>>();

        private void Start()
        {
            Map = AppCache.CityBuilding.MyCompCity.PathFindingMap;
            var wall = AppCache.CityBuilding.MyCompCity.GetBuilding(EBuildingType.CityWall);
            if (wall == null || wall.Body == null)
            {
                return;
            }
            
            for (var i = 0; i < Map.RowCount; i++)
            {
                var list = new List<GameObject>();
                for (var j = 0; j < Map.ColumnCount; j++)
                {
                    var cellObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    //cellObj.transform.parent = transform;
                    var v2Pos = Map.StartPos + CoordHelper.GridToCartesianCoord(new int2(i, j)).ToVector2();
                                      
                    // var pos = CoordHelper.GridToCartesianCoord(new int2(i, j));
                    // pos += wall.Owner.LocalCenterOffset + wall.Owner.Trans.position;
                    cellObj.transform.position = v2Pos.ToVector3(-0.02f);
                    cellObj.transform.localScale = new Vector3(0.05f, 0.05f, .05f);
                    cellObj.transform.localRotation = Quaternion.Euler(new Vector3(0f, -45f, 0f));
                    var msRender = cellObj.GetComponent<MeshRenderer>();
                    msRender.material.shader = Shader.Find("Transparent/Diffuse");
                    // 设置物体的初始颜色
                    var color = Color.green;
                    color.a = 0.3f;
                    msRender.material.color = color;
                    cellObj.name = $"[{i},{j}] [{i * Map.ColumnCount + j}]";
                    list.Add(cellObj);
                }

                CellList.Add(list);
            }
        }


        public void Update()
        {
            if (Time.frameCount % 15 == 0)
            {
                for (var i = 0; i < Map.RowCount; i++)
                {
                    for (var j = 0; j < Map.ColumnCount; j++)
                    {
                        var mapCell = Map.GetCell(i, j);
                        var cellObj = CellList[i][j];

                        var color = mapCell.HasObstacle ? Color.red : Color.green;
                        color.a = 0.1f;
                        var msRender = cellObj.GetComponent<MeshRenderer>();
                        // 设置物体的初始颜色
                        msRender.material.color = color;
                    }
                }
            }
        }
    }
}