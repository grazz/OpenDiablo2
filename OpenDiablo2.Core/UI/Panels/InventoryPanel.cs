﻿/*  OpenDiablo 2 - An open source re-implementation of Diablo 2 in C#
 *  
 *   This program is free software: you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation, either version 3 of the License, or
 *   (at your option) any later version.
 *
 *   This program is distributed in the hope that it will be useful,
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *   GNU General Public License for more details.
 *
 *   You should have received a copy of the GNU General Public License
 *   along with this program.  If not, see <https://www.gnu.org/licenses/>. 
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OpenDiablo2.Common;
using OpenDiablo2.Common.Enums;
using OpenDiablo2.Common.Extensions;
using OpenDiablo2.Common.Interfaces;
using OpenDiablo2.Common.Models;

namespace OpenDiablo2.Core.UI
{
    /**
     * TODO: Add logic so it can be used as an element in inventory grid
     **/
    public sealed class InventoryPanel : IInventoryPanel
    {
        private readonly IRenderWindow renderWindow;
        private readonly IMapRenderer mapRenderer;
        private readonly ISprite panelSprite;
        private readonly IGameState gameState;

        public IItemContainer headContainer, torsoContainer, beltContainer, gloveContainer, bootsContainer,
            leftHandContainer, rightHandContainer, secondaryLeftHandContainer, secondaryRightHandContainer,
            ringLeftContainer, ringRightContainer, neckContainer;

        private readonly IButton closeButton, secondaryLeftButton, secondaryRightButton, goldButton;

        public event OnPanelClosedEvent OnPanelClosed;

        public InventoryPanel(IRenderWindow renderWindow, 
            IItemManager itemManager, 
            IMapRenderer mapRenderer,
            ISessionManager sessionManager,
            Func<eItemContainerType, IItemContainer> createItemContainer,
            IGameState gameState,
            Func<eButtonType, IButton> createButton)
        {
            this.renderWindow = renderWindow;
            this.mapRenderer = mapRenderer;
            this.gameState = gameState;

            sessionManager.OnFocusOnPlayer += OnFocusOnPlayer;
            sessionManager.OnPlayerInfo += OnPlayerInfo;

            panelSprite = renderWindow.LoadSprite(ResourcePaths.InventoryCharacterPanel, Palettes.Units, FrameType.GetOffset(), true);

            closeButton = createButton(eButtonType.Close);
            closeButton.Location = panelSprite.Location + new Size(18, 384);
            closeButton.OnActivate = () => OnPanelClosed?.Invoke(this);

            secondaryLeftButton = createButton(eButtonType.SecondaryInvHand);
            secondaryLeftButton.Location = panelSprite.Location + new Size(15, 22);
            secondaryLeftButton.OnActivate = ToggleWeaponsSlot;

            secondaryRightButton = createButton(eButtonType.SecondaryInvHand);
            secondaryRightButton.Location = panelSprite.Location + new Size(246, 22);
            secondaryRightButton.OnActivate = ToggleWeaponsSlot;

            goldButton = createButton(eButtonType.GoldCoin);
            goldButton.Location = panelSprite.Location + new Size(84, 391);
            goldButton.OnActivate = OpenGoldDrop;

            headContainer = createItemContainer(eItemContainerType.Helm);
            headContainer.Location = panelSprite.Location + new Size(135, 5);
            
            neckContainer = createItemContainer(eItemContainerType.Amulet);
            neckContainer.Location = panelSprite.Location + new Size(209, 34);
            
            torsoContainer = createItemContainer(eItemContainerType.Armor);
            torsoContainer.Location = panelSprite.Location + new Size(135, 75);

            rightHandContainer = createItemContainer(eItemContainerType.Weapon);
            rightHandContainer.Location = panelSprite.Location + new Size(20, 47);

            leftHandContainer = createItemContainer(eItemContainerType.Weapon);
            leftHandContainer.Location = panelSprite.Location + new Size(253, 47);

            secondaryLeftHandContainer = createItemContainer(eItemContainerType.Weapon);
            secondaryLeftHandContainer.Location = panelSprite.Location + new Size(24, 45);

            secondaryRightHandContainer = createItemContainer(eItemContainerType.Weapon);
            secondaryRightHandContainer.Location = panelSprite.Location + new Size(257, 45);

            beltContainer = createItemContainer(eItemContainerType.Belt);
            beltContainer.Location = panelSprite.Location + new Size(136, 178);
            
            ringLeftContainer = createItemContainer(eItemContainerType.Ring);
            ringLeftContainer.Location = panelSprite.Location + new Size(95, 179);
            
            ringRightContainer = createItemContainer(eItemContainerType.Ring);
            ringRightContainer.Location = panelSprite.Location + new Size(209, 179);
            
            gloveContainer = createItemContainer(eItemContainerType.Glove);
            gloveContainer.Location = panelSprite.Location + new Size(20, 179);
            
            bootsContainer = createItemContainer(eItemContainerType.Boots);
            bootsContainer.Location = panelSprite.Location + new Size(251, 178);
        }

        private void OnPlayerInfo(int clientHash, IEnumerable<PlayerInfo> playerInfo)
        {
            var currentPlayer = gameState.PlayerInfos.FirstOrDefault(x => x.UID == mapRenderer.FocusedPlayerId);
            if (currentPlayer != null)
                UpdateInventoryPanel(currentPlayer);
        }

        private void OnFocusOnPlayer(int clientHash, Guid playerId)
        {
            var currentPlayer = gameState.PlayerInfos.FirstOrDefault(x => x.UID == playerId);
            if (currentPlayer != null)
                UpdateInventoryPanel(currentPlayer);
        }

        private void UpdateInventoryPanel(PlayerInfo currentPlayer)
        {
            leftHandContainer.SetContainedItem(currentPlayer.Equipment.LeftArm);
            rightHandContainer.SetContainedItem(currentPlayer.Equipment.RightArm);
            torsoContainer.SetContainedItem(currentPlayer.Equipment.Torso);
            headContainer.SetContainedItem(currentPlayer.Equipment.Head);
            ringLeftContainer.SetContainedItem(currentPlayer.Equipment.LeftRing);
            ringRightContainer.SetContainedItem(currentPlayer.Equipment.RightRing);
            beltContainer.SetContainedItem(currentPlayer.Equipment.Belt);
            neckContainer.SetContainedItem(currentPlayer.Equipment.Neck);
            gloveContainer.SetContainedItem(currentPlayer.Equipment.Gloves);
        }

        public ePanelType PanelType => ePanelType.Inventory;
        public ePanelFrameType FrameType => ePanelFrameType.Right;

        public bool IsSecondaryEquipped { get; private set; }

        public void Update()
        {
            if (IsSecondaryEquipped)
            {
                secondaryLeftHandContainer.Update();
                secondaryRightHandContainer.Update();
            }
            else
            {
                leftHandContainer.Update();
                rightHandContainer.Update();
            }

            secondaryLeftButton.Update();
            secondaryRightButton.Update();

            closeButton.Update();
            goldButton.Update();

            headContainer.Update();
            neckContainer.Update();
            torsoContainer.Update();
            beltContainer.Update();
            ringLeftContainer.Update();
            ringRightContainer.Update();
            gloveContainer.Update();
            bootsContainer.Update();
        }

        public void Render()
        {
            renderWindow.Draw(panelSprite, 2, 2, 1);

            if (IsSecondaryEquipped)
            {
                secondaryLeftButton.Render();
                secondaryRightButton.Render();
                secondaryLeftHandContainer.Render();
                secondaryRightHandContainer.Render();
            }
            else
            {
                leftHandContainer.Render();
                rightHandContainer.Render();
            }

            closeButton.Render();
            goldButton.Render();

            headContainer.Render();
            neckContainer.Render();
            torsoContainer.Render();
            beltContainer.Render();
            ringLeftContainer.Render();
            ringRightContainer.Render();
            gloveContainer.Render();
            bootsContainer.Render();
        }

        public void Dispose()
        {
            panelSprite.Dispose();
        }

        private void ToggleWeaponsSlot()
        {
            IsSecondaryEquipped = !IsSecondaryEquipped;
        }

        private void OpenGoldDrop()
        {

        }
    }
}
