﻿using System;
using System.Drawing;
using OpenDiablo2.Common;
using OpenDiablo2.Common.Enums;
using OpenDiablo2.Common.Interfaces;

namespace OpenDiablo2.Core.UI
{
    public sealed class CharacterPanel : ICharacterPanel
    {
        private readonly IRenderWindow renderWindow;
        private ISprite sprite, framesprite;

        private Point location = new Point();
        public Point Location
        {
            get => location;
            set
            {
                if (location == value)
                    return;
                location = value;
            }
        }

        public CharacterPanel(IRenderWindow renderWindow)
        {
            this.renderWindow = renderWindow;
            
            framesprite = renderWindow.LoadSprite(ResourcePaths.Frame, Palettes.Units, new Point(0, 0));

            sprite = renderWindow.LoadSprite(ResourcePaths.InventoryCharacterPanel, Palettes.Units, new Point(79,61));
            Location = new Point(0, 0);

           
        }

        private void DrawPanel()
        {
            renderWindow.Draw(framesprite, 0, new Point(0,256));
            renderWindow.Draw(framesprite, 1, new Point(256, 66));
            renderWindow.Draw(framesprite, 2, new Point(0, 256+231));
            renderWindow.Draw(framesprite, 3, new Point(0, 256 + 231 + 66));
            renderWindow.Draw(framesprite, 4, new Point(256, 256 + 231 + 66));
            renderWindow.Draw(sprite, 2, 2, 0);
        }


        public void Update()
        {
            
            
        }

        public void Render()
        {
            DrawPanel();
        }

        public void Dispose()
        {
            sprite.Dispose();
        }
    }
}
