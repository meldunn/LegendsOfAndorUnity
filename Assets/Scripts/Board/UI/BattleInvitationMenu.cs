using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleInvitationMenu : MonoBehaviour, Observer
{
    // References to managers
    private GameManager GameManager;

    // Observer hero
    Hero MyHero;

    // References to children components
    [SerializeField]
    GameObject BattleInviteText = null;
    [SerializeField]
    GameObject BattleInviteDeclineButton = null;
    [SerializeField]
    GameObject BattleInviteAcceptButton = null;
    [SerializeField]
    GameObject BattleInviteSpinner = null;

    // Start is called before the first frame update
    void Start()
    {
        // Not used to ensure UI elements are initialized in the right order.
        // UIManager calls Initialize instead.
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize()
    {
        // Initialize references to managers
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        // Register as an observer of GameManager
        GameManager.Attach(this);

        // Register as an observer of the controlled hero
        MyHero = GameManager.GetSelfHero();
        MyHero.Attach(this);
    }

    // Shows this menu
    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    // Hides this menu
    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    // Initializes the visual content in this menu
    private void InitializeUI()
    {
        // Initialize UI
        SetInfoText("");
        EnableButton(BattleInviteAcceptButton);
        EnableButton(BattleInviteDeclineButton);
        BattleInviteSpinner.SetActive(false);
    }

    // Used in Observer design pattern
    public void UpdateData(string Category)
    {
        if (string.Equals(Category, "INVITE_STATUS"))
        {
            UpdateInviteMenu();
        }
        else if (string.Equals(Category, "CONTROL"))
        {
            // When changing the controlled hero, change which hero is being observed and re-initialize this menu
            
            // Unregister as an observer of the old hero
            MyHero.Detach(this);

            // Register as an observer of the newly controlled hero
            MyHero = GameManager.GetSelfHero();
            MyHero.Attach(this);

            // Reset this menu
            UpdateInviteMenu();
        }
    }

    private void SetInfoText(string Text)
    {
        TextMeshProUGUI InfoText = BattleInviteText.GetComponent<TextMeshProUGUI>();
        InfoText.SetText(Text);
    }

    public void EnableButton(GameObject Button)
    {
        Button.GetComponent<Button>().interactable = true;
    }

    public void DisableButton(GameObject Button)
    {
        Button.GetComponent<Button>().interactable = false;
    }

    private void UpdateInviteMenu()
    {
        // Defaults
        InitializeUI();

        // Get a reference to the battle invitation
        BattleInvitation Invite = MyHero.GetBattleInvitation();

        // Check the status of the invitation
        if (Invite == null)
        {
            this.Hide();
        }
        else
        {
            if (Invite.IsPending())
            {
                // Generate a text string with the battle details
                HeroType HeroType = Invite.GetBattleStarter().GetHeroType();
                CreatureType CreatureType = Invite.GetCreature().GetCreatureType();
                int RegionNum = Invite.GetCreature().GetRegion().GetWaypointNum();

                string BattleDetails = "The " + HeroType + " has invited you to fight the " + CreatureType + " on region " + RegionNum + ". Do you want to join them?";

                // Set the text contents in the viewer
                SetInfoText(BattleDetails);

                // Show this menu
                this.Show();
            }
            else if (Invite.WasAccepted())
            {
                // Disable the buttons
                DisableButton(BattleInviteAcceptButton);
                DisableButton(BattleInviteDeclineButton);

                // Display a screen asking the player to wait for everyone to accept their battle invitations
                string PleaseWait = "Please wait for all invited players to accept their invitations...";
                SetInfoText(PleaseWait);

                // Show the spinner
                BattleInviteSpinner.SetActive(true);

                // Show this menu
                this.Show();
            }
            else if (Invite.WasDeclined())
            {
                this.Hide();
            }
        }
    }

    public void AcceptInvitation()
    {
        // Get a reference to the battle invitation
        BattleInvitation Invite = MyHero.GetBattleInvitation();

        Invite.Accept();
    }

    public void DeclineInvitation()
    {
        // Get a reference to the battle invitation
        BattleInvitation Invite = MyHero.GetBattleInvitation();

        Invite.Decline();
    }
}
