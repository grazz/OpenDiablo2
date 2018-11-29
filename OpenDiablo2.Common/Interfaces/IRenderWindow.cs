﻿using System;
using System.Drawing;
using OpenDiablo2.Common.Models;

namespace OpenDiablo2.Common.Interfaces
{
    public interface IRenderWindow : IDisposable
    {
        bool IsRunning { get; }
        void Update();
        void Clear();
        void Sync();
        void Quit();
        ISprite LoadSprite(string resourcePath, string palette, Point location);
        ISprite LoadSprite(string resourcePath, string palette);
        IFont LoadFont(string resourcePath, string palette);
        ILabel CreateLabel(IFont font);
        ILabel CreateLabel(IFont font, string text);
        ILabel CreateLabel(IFont font, Point position, string text);
        void Draw(ISprite sprite);
        void Draw(ISprite sprite, Point location);
        void Draw(ISprite sprite, int frame, Point location);
        void Draw(ISprite sprite, int frame);
        void Draw(ISprite sprite, int xSegments, int ySegments, int offset);
        IMouseCursor LoadCursor(ISprite sprite, int frame, Point hotspot);
        void SetCursor(IMouseCursor mouseCursor);
        void Draw(ILabel label);
        MapCellInfo CacheMapCell(MPQDT1Tile mapCell);
        void DrawMapCell(MapCellInfo mapCellInfo, int xPixel, int yPixel);
    }
}
