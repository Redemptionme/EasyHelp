using System;
using System.Collections;
using System.Collections.Generic;
using HHL.Game;
using IGG.Framework.Panel;
using IGG.Game.Module.BattleRoyale;
using IGG.Game.Module.BattleRoyale.View;
using IGG.Game.Module.CampIsland;
using IGG.Game.Module.Common.View;
using IGG.Game.Module.WorldMap.Help;
using UnityEngine;

namespace HHL.Common
{
    public class HHLGOTools : MonoBehaviour
    {
        // Start is called before the first frame update
        private void Start()
        {
            DontDestroyOnLoad(this);
        }

        // Update is called once per frame
        private void Update()
        {
            CheckInput();
        }

        private void OnDestroy()
        {
            HHL.Common.Log.Inst.ToolsInit = false;
        }

        private void CheckInput()
        {
            if (Input.GetKeyDown(KeyCode.F4))
            {
                //gameObject.AddComponent<CityMapTool>();
                //BattleRoyaleModule.Inst.OpenLoading();
                //CampIslandModule.Inst.OpenActivityPanel();
                //CampIslandModule.Inst.OpenInnerActivityRank();
            }
            
            if (Input.GetKeyDown(KeyCode.F3))
            {
                //PanelMgr.Inst.ClosePanel<BattleRoyaleLoadingPanel>();
            }
            
            
            
        }
    }
}