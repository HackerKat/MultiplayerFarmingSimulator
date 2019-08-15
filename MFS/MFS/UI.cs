using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// using GeonBit UI elements
using GeonBit.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GBBtutton = GeonBit.UI.Entities.Button;
using GBEntity = GeonBit.UI.Entities.Entity;

namespace MFS
{
    public class UI
    {
        private GBBtutton clientButton;
        private GBBtutton hostButton;
        private Panel startScreenPanel;
        private TextInput input;

        private GBBtutton inventoryButton;
        private Panel inventoryPanel;

        public delegate void HostButtonOnClick();
        public delegate void ConnectButtonOnClick(string ipAdress);
        public delegate void OpenInventoryToggle(Panel inventoryPanel);
        public event HostButtonOnClick OnHost;
        public event ConnectButtonOnClick OnConnect;
        public event OpenInventoryToggle OnInventory;

        public UI(ContentManager content)
        {
            UserInterface.Initialize(content, BuiltinThemes.hd);
            UserInterface.Active.UseRenderTarget = true;
            CreateStartMenu();
            CreateInGameUI();
        }

        public void CreateStartMenu()
        {
            startScreenPanel = new Panel();
            UserInterface.Active.AddEntity(startScreenPanel);

            hostButton = new GBBtutton("Host", skin: ButtonSkin.Alternative, anchor: Anchor.Auto, size: new Vector2(300, 50));
            startScreenPanel.AddChild(hostButton);
            hostButton.OnClick += StartHost;

            input = new TextInput(false, anchor: Anchor.Auto, size: new Vector2(300, 50));
            input.PlaceholderText = "Insert Host IP";
            startScreenPanel.AddChild(input);
            clientButton = new GBBtutton("Connect to host", skin: ButtonSkin.Alternative, anchor: Anchor.Auto, size: new Vector2(300, 50));
            startScreenPanel.AddChild(clientButton);
            clientButton.OnClick += ConnectToHost;
        }

        public void CreateInGameUI()
        {
            inventoryPanel = new Panel(new Vector2(300, 300));
            UserInterface.Active.AddEntity(inventoryPanel);
            inventoryPanel.PanelOverflowBehavior = PanelOverflowBehavior.VerticalScroll;
            inventoryPanel.Visible = false;


            inventoryButton = new GBBtutton("Inventory", skin: ButtonSkin.Alternative, anchor: Anchor.BottomRight, size: new Vector2(300, 50));
            inventoryButton.ToggleMode = true;
            inventoryButton.OnValueChange = OpenInventory;
            UserInterface.Active.AddEntity(inventoryButton);
            inventoryButton.Visible = false;
        }

        public void OpenInventory(GBEntity entity)
        {
            if (inventoryButton.Checked)
            {
                if (OnInventory != null)
                {
                    OnInventory(inventoryPanel);
                }
                inventoryPanel.Visible = true;
            }
            else
            {
                inventoryPanel.Visible = false;
            }
        }

        public void EnableStartScreen()
        {
            startScreenPanel.Visible = true;
        }

        public void DissableStartScreen()
        {
            startScreenPanel.Visible = false;
            inventoryButton.Visible = true;
        }

        public void EnableInGameUI()
        {
            inventoryPanel.Visible = false;
            inventoryButton.Visible = true;
        }

        public void DissableInGameUI()
        {

        }
        

        public void ConnectToHost(GBEntity btn)
        {
            if (OnConnect != null)
            {
                string ipAdress = input.Value;
                OnConnect(ipAdress);
            }
        }

        public void StartHost(GBEntity btn)
        {
            if (OnHost != null)
            {
                OnHost();
            }
        }

        public void CreateInventory()
        {

        }

        public void BeginDraw(SpriteBatch spriteBatch)
        {
            UserInterface.Active.Draw(spriteBatch);
        }

        public void EndDraw(SpriteBatch spriteBatch)
        {
            UserInterface.Active.DrawMainRenderTarget(spriteBatch);
        }
    }
}
