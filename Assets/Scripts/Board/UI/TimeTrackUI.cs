using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTrackUI : MonoBehaviour, Observer
{
    // References to managers
    private GameManager GameManager;
    private HeroManager HeroManager;

    // References to children components
    // Time markers
    [SerializeField]
    GameObject WarriorTimeMarker = null;
    [SerializeField]
    GameObject ArcherTimeMarker = null;
    [SerializeField]
    GameObject DwarfTimeMarker = null;
    [SerializeField]
    GameObject WizardTimeMarker = null;

    // Rooster box
    [SerializeField]
    GameObject RoosterBox = null;

    // Array of sections in each hour box (hour 0 represents the sunrise box)
    [SerializeField]
    GameObject[] HourBox0 = null;
    [SerializeField]
    GameObject[] HourBox1 = null;
    [SerializeField]
    GameObject[] HourBox2 = null;
    [SerializeField]
    GameObject[] HourBox3 = null;
    [SerializeField]
    GameObject[] HourBox4 = null;
    [SerializeField]
    GameObject[] HourBox5 = null;
    [SerializeField]
    GameObject[] HourBox6 = null;
    [SerializeField]
    GameObject[] HourBox7 = null;
    [SerializeField]
    GameObject[] HourBox8 = null;
    [SerializeField]
    GameObject[] HourBox9 = null;
    [SerializeField]
    GameObject[] HourBox10 = null;

    // 2D array that is assembled from the arrays above (2D arrays are not supported in SerizalizeFields)
    GameObject[][] HourBox = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize()
    {
        // Initialize references to managers
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        HeroManager = GameObject.Find("HeroManager").GetComponent<HeroManager>();

        // Register as an observer of the four heroes
        Hero Warrior = HeroManager.GetHero(HeroType.Warrior);
        Hero Archer = HeroManager.GetHero(HeroType.Archer);
        Hero Dwarf = HeroManager.GetHero(HeroType.Dwarf);
        Hero Wizard = HeroManager.GetHero(HeroType.Wizard);

        Warrior.Attach(this);
        Archer.Attach(this);
        Dwarf.Attach(this);
        Wizard.Attach(this);

        // Initialize the 2D array of time marker slots
        HourBox = new GameObject[][] {
            HourBox0,
            HourBox1,
            HourBox2,
            HourBox3,
            HourBox4,
            HourBox5,
            HourBox6,
            HourBox7,
            HourBox8,
            HourBox9,
            HourBox10
        };

        InitializeUI();
    }

    // Used in Observer design pattern
    public void UpdateData(string Category)
    {
        if (string.Equals(Category, "HERO_TIME"))
        {
            UpdateTimeTrack();
        }
    }

    private void InitializeUI()
    {
        UpdateTimeTrack();
    }

    // Refreshes the time track completely
    private void UpdateTimeTrack()
    {
        List<HeroType> AllHeroTypes = HeroManager.GetAllHeroTypes();

        foreach (HeroType HeroType in AllHeroTypes)
        {
            // Determine whether the hero is playing
            if (GameManager.IsPlaying(HeroType))
            {
                // Get the hero's time value
                int Hour = HeroManager.GetHero(HeroType).GetTimeOfDay();

                // Move their marker to the right space
                MoveTimeMarker(HeroType, Hour);
            }
            // If the hero isn't playing, hide their time marker
            else
            {
                TimeMarker(HeroType).SetActive(false);
            }
        }
    }

    private GameObject TimeMarker(HeroType Type)
    {
        if (Type == HeroType.Warrior) return WarriorTimeMarker;
        else if (Type == HeroType.Archer) return ArcherTimeMarker;
        else if (Type == HeroType.Dwarf) return DwarfTimeMarker;
        else if (Type == HeroType.Wizard) return WizardTimeMarker;
        else return null;
    }

    private void MoveTimeMarker(HeroType Type, int Hour)
    {
        // Validate time
        if (Hour < 0 || Hour > 10)
        {
            Debug.LogError("Cannot update " + Type + "'s time marker on the time track; invalid hour value " + Hour);
            return;
        }

        // Validate hero and set the hour box section index
        int Section = -1;
        if (Type == HeroType.Warrior) Section = 0;
        else if (Type == HeroType.Archer) Section = 1;
        else if (Type == HeroType.Dwarf) Section = 2;
        else if (Type == HeroType.Wizard) Section = 3;
        else
        {
            Debug.LogError("Cannot update time marker for invalid HeroType" + Type);
            return;
        }

        Hero Hero = HeroManager.GetHero(Type);

        // The new location of the time marker
        Vector3 NewLocation;

        // Determine whether to place the marker in the rooster box
        if (Hour == 0 && Hero.IsInRoosterBox())
        {
            NewLocation = RoosterBox.transform.position;
        }
        else
        {
            // Otherwise, place the time marker in the correct section of the right hour box
            NewLocation = HourBox[Hour][Section].transform.position;
        }

        // Move the marker
        TimeMarker(Type).transform.SetPositionAndRotation(NewLocation,      // Destination
            Quaternion.identity);                                           // No rotation
    }
}
