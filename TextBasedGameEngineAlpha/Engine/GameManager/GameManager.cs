using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TextBasedGameEngine.Engine.Classes;
using TextBasedGameEngine.Engine.RenderSystem;
using TextBasedGameEngineAlpha.Engine.GameManager;
using TextBasedGameEngineAlpha.Engine.GameManager.WorldEntities;

namespace TextBasedGameEngine.Engine.GameManager
{
    public class HL_GameManager 
    {
        public HL_LocalPlayerHandler LocalPlayer = null;
        public HL_MapManager MapManager = null;
        public HL_EntityHandler EntityHandler = null;
        public HL_GameMovement GameMovement = null;

        public void Initialize(HL_Engine _OwningEngine)
        {
            MapManager = new HL_MapManager();
            LocalPlayer = new HL_LocalPlayerHandler();
            EntityHandler = new HL_EntityHandler();
            GameMovement = new HL_GameMovement();

            OwningEngine = _OwningEngine;
            EntityHandler.Initialize(_OwningEngine);
            GameMovement.Initialize(_OwningEngine);
            MapManager.Initialize(_OwningEngine);
        }

        private HL_Engine OwningEngine = null;
    }
}
