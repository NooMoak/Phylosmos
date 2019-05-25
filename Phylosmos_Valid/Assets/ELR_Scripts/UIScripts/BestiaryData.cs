using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BestiaryData : MonoBehaviour
{
    public bool spike1 = false;
    public bool spike2 = false;
    public bool spike3 = false;
    public bool liana1 = false;
    public bool liana2 = false;
    public bool liana3 = false;
    public bool healer1 = false;
    public bool healer2 = false;
    public bool healer3 = false;
    public bool rock1 = false;
    public bool rock2 = false;
    public bool rock3 = false;
    public bool villager1 = false;
    public bool villager2 = false;
    public bool villager3 = false;
    public bool villager4 = false;
    public bool villager5 = false;
    public bool villagerField = false;
    public bool villagerTower = false;
    public bool villagerLanguage = false;
    public bool villagerDrawings = false;
    public bool villagerChief = false;
    public bool boss1 = false;
    public bool boss2 = false;
    public bool boss3 = false;
    public bool boss4 = false;
    DataSaver dataSaver;
    [SerializeField] GameObject analysisUI;
    [SerializeField] Image analysisJauge;
    [SerializeField] Text analysisText;
    [SerializeField] Text analysisStep;

    [SerializeField] Text[] nativesButtons;
    [SerializeField] Text[] lianaButtons;
    [SerializeField] Text[] spikeButtons;
    [SerializeField] Text[] healerButtons;
    [SerializeField] Text[] rockButtons;
    [SerializeField] Text[] bossButtons;

    [SerializeField] Text nativesTitle;
    [SerializeField] RawImage nativesImage;
    [SerializeField] Text nativesLanguage;
    [SerializeField] Text nativesChief;
    [SerializeField] Text nativesDrawings;
    [SerializeField] Text nativesTower;
    [SerializeField] Text nativesLore;
    [SerializeField] Text nativesField;

    [SerializeField] Text lianaTitle;
    [SerializeField] Text lianaName;
    [SerializeField] RawImage lianaImage;
    [SerializeField] Text lianaWR;
    [SerializeField] Text lianaLore;

    [SerializeField] Text spikeTitle;
    [SerializeField] Text spikeName;
    [SerializeField] RawImage spikeImage;
    [SerializeField] Text spikeWR;
    [SerializeField] Text spikeLore;

    [SerializeField] Text healerTitle;
    [SerializeField] Text healerName;
    [SerializeField] RawImage healerImage;
    [SerializeField] Text healerWR;
    [SerializeField] Text healerLore;

    [SerializeField] Text rockTitle;
    [SerializeField] Text rockName;
    [SerializeField] RawImage rockImage;
    [SerializeField] Text rockWR;
    [SerializeField] Text rockLore;

    [SerializeField] Text bossTitle;
    [SerializeField] Text bossName;
    [SerializeField] RawImage bossImage;
    [SerializeField] Text bossWR;
    [SerializeField] Text bossLore;
    [SerializeField] Text bossEnd;

    [SerializeField] Text introText;

    float targetedAmount = 0f;
    // Start is called before the first frame update
    void Start()
    {
        dataSaver = FindObjectOfType<DataSaver>();
        spike1 = dataSaver.spike1;
        spike2 = dataSaver.spike2;
        spike3 = dataSaver.spike3;
        liana1 = dataSaver.liana1;
        liana2 = dataSaver.liana2;
        liana3 = dataSaver.liana3;
        healer1 = dataSaver.healer1;
        healer2 = dataSaver.healer2;
        healer3 = dataSaver.healer3;
        rock1 = dataSaver.rock1;
        rock2 = dataSaver.rock2;
        rock3 = dataSaver.rock3;
        villager1 = dataSaver.villager1;
        villager2 = dataSaver.villager2;
        villager3 = dataSaver.villager3;
        villager4 = dataSaver.villager4;
        villager5 = dataSaver.villager5;
        villagerField = dataSaver.villagerField;
        villagerTower = dataSaver.villagerTower;
        villagerLanguage = dataSaver.villagerLanguage;
        villagerDrawings = dataSaver.villagerDrawings;
        villagerChief = dataSaver.villagerChief;
        boss1 = dataSaver.boss1;
        boss2 = dataSaver.boss2;
        boss3 = dataSaver.boss3;
        boss4 = dataSaver.boss4;

        if(spike1 == true || liana1 == true || healer1 == true || rock1 == true)
        {
            introText.gameObject.SetActive(false);
        }

        UpdateBestiary();
    }

    // Update is called once per frame
    void Update()
    {
        if(analysisJauge.fillAmount != targetedAmount)
        {
            analysisJauge.fillAmount = Mathf.Lerp(analysisJauge.fillAmount, targetedAmount, 0.1f);
        }
    }

    public void SpikeEntry()
    {
        analysisUI.SetActive(true);
        StartCoroutine(Stop());
        if(spike1 == false)
        {
            spike1 = true;
            dataSaver.spike1 = true;
            analysisText.text = "Thorny_analysis";
            analysisStep.text = "1 / 3 step";
            targetedAmount = 0.33f;
        }
        else if(spike2 == false)
        {
            spike2 = true;
            dataSaver.spike2 = true;
            analysisText.text = "Thorny_analysis";
            analysisStep.text = "2 / 3 step";
            targetedAmount = 0.66f;
        }
        else if(spike3 == false)
        {
            spike3 = true;
            dataSaver.spike3 = true;
            analysisText.text = "Thorny_analysis";
            analysisStep.text = "3 / 3 step";
            targetedAmount = 1f;
        }
        UpdateBestiary();
    }

    public void LianaEntry()
    {
        analysisUI.SetActive(true);
        StartCoroutine(Stop());
        if(liana1 == false)
        {
            liana1 = true;
            dataSaver.liana1 = true;
            analysisText.text = "Liana_analysis";
            analysisStep.text = "1 / 3 step";
            targetedAmount = 0.33f;
        }
        else if(liana2 == false)
        {
            liana2 = true;
            dataSaver.liana2 = true;
            analysisText.text = "Liana_analysis";
            analysisStep.text = "2 / 3 step";
            targetedAmount = 0.66f;
        }
        else if(liana3 == false)
        {
            liana3 = true;
            dataSaver.liana3 = true;
            analysisText.text = "Liana_analysis";
            analysisStep.text = "3 / 3 step";
            targetedAmount = 1f;
        }
        UpdateBestiary();
    }
    
    public void HealerEntry()
    {
        analysisUI.SetActive(true);
        StartCoroutine(Stop());
        if(healer1 == false)
        {
            healer1 = true;
            dataSaver.healer1 = true;
            analysisText.text = "Healer_analysis";
            analysisStep.text = "1 / 3 step";
            targetedAmount = 0.33f;
        }
        else if(healer2 == false)
        {
            healer2 = true;
            dataSaver.healer2 = true;
            analysisText.text = "Healer_analysis";
            analysisStep.text = "2 / 3 step";
            targetedAmount = 0.66f;
        }
        else if(healer3 == false)
        {
            healer3 = true;
            dataSaver.healer3 = true;
            analysisText.text = "Healer_analysis";
            analysisStep.text = "3 / 3 step";
            targetedAmount = 1f;
        }
        UpdateBestiary();
    }
    
    public void RockEntry()
    {
        analysisUI.SetActive(true);
        StartCoroutine(Stop());
        if(rock1 == false)
        {
            rock1 = true;
            dataSaver.rock1 = true;
            analysisText.text = "Guardian_analysis";
            analysisStep.text = "1 / 3 step";
            targetedAmount = 0.33f;
        }
        else if(rock2 == false)
        {
            rock2 = true;
            dataSaver.rock2 = true;
            analysisText.text = "Guardian_analysis";
            analysisStep.text = "2 / 3 step";
            targetedAmount = 0.66f;
        }
        else if(rock3 == false)
        {
            rock3 = true;
            dataSaver.rock3 = true;
            analysisText.text = "Guardian_analysis";
            analysisStep.text = "3 / 3 step";
            targetedAmount = 1f;
        }
        UpdateBestiary();
    }
    
    public void VillagerEntry()
    {
        analysisUI.SetActive(true);
        StartCoroutine(Stop());
        if(villager1 == false)
        {
            villager1 = true;
            dataSaver.villager1 = true;
            analysisText.text = "Natives_analysis";
            analysisStep.text = "1 / 5 step";
            targetedAmount = 0.2f;
        }
        else if(villager2 == false)
        {
            villager2 = true;
            dataSaver.villager2 = true;
            analysisText.text = "Natives_analysis";
            analysisStep.text = "2 / 5 step";
            targetedAmount = 0.4f;
        }
        else if(villager3 == false)
        {
            villager3 = true;
            dataSaver.villager3 = true;
            analysisText.text = "Natives_analysis";
            analysisStep.text = "3 / 5 step";
            targetedAmount = 0.6f;
        }
        else if(villager4 == false)
        {
            villager4 = true;
            dataSaver.villager4 = true;
            analysisText.text = "Natives_analysis";
            analysisStep.text = "4 / 5 step";
            targetedAmount = 0.8f;
        }
        else if(villager5 == false)
        {
            villager5 = true;
            dataSaver.villager5 = true;
            analysisText.text = "Natives_analysis";
            analysisStep.text = "5 / 5 step";
            targetedAmount = 1f;
        }
        UpdateBestiary();
    }
    
    public void BossEntry()
    {
        analysisUI.SetActive(true);
        StartCoroutine(Stop());
        if(boss1 == false)
        {
            boss1 = true;
            dataSaver.boss1 = true;
            analysisText.text = "FireBird_analysis";
            analysisStep.text = "1 / 4 step";
            targetedAmount = 0.25f;
        }
        else if(boss2 == false)
        {
            boss2 = true;
            dataSaver.boss2 = true;
            analysisText.text = "FireBird_analysis";
            analysisStep.text = "2 / 4 step";
            targetedAmount = 0.5f;
        }
        else if(boss3 == false)
        {
            boss3 = true;
            dataSaver.boss3 = true;
            analysisText.text = "FireBird_analysis";
            analysisStep.text = "3 / 4 step";
            targetedAmount = 0.75f;
        }
        else if(boss4 == false)
        {
            boss4 = true;
            dataSaver.boss4 = true;
            analysisText.text = "FireBird_analysis";
            analysisStep.text = "4 / 4 step";
            targetedAmount = 1f;
        }
        UpdateBestiary();
    }

    public void EndEntry()
    {
        analysisUI.SetActive(true);
        StartCoroutine(Stop());
        analysisText.text = "The truth";
        analysisStep.text = "1 / 1 step";
        targetedAmount = 1f;
    }

    IEnumerator Stop()
    {
        yield return new WaitForSeconds(3);
        analysisUI.SetActive(false);
        targetedAmount = 0;
    }

    void UpdateBestiary()
    {
        if(spike1 == false)
        {
            foreach(Text text in spikeButtons)
            {
                text.text = "???";
            }
            spikeTitle.text = "???";
            spikeName.text = "???";
            spikeImage.enabled = false;
            spikeWR.text = "???";
            spikeLore.text = "???";
            introText.text = "This is your DataBase, it's automatically filled by Syntro each time you analyze elements of this planet. It contains important information that allow you to better understand your environment, read your DataBase will help surviving during your adventure.\nYou can access it at any time by pressing \"TAB\".\nPress \"Escape\" to quit the DataBase.";
        }
        if(spike1 == true)
        {
            foreach(Text text in spikeButtons)
            {
                text.text = "Thorny imitator";
            }
            spikeTitle.text = "Thorny imitator";
            spikeWR.text = "His main strength is based on the distance he can put between him and his prey.\nIn addition, he will try to dissuade anyone from approaching by firing bursts of spades.";
            introText.text = "This is your DataBase, it's automatically filled by Syntro each time you analyze elements of this planet. It contains important information that allow you to better understand your environment, read your DataBase will help surviving during your adventure.\nHere, Syntro discovered that this spike belonged to a living creature, by analyzing it, Syntro could find some precious information about this creature.\nKeep analyzing in order to complete your DataBase.\nYou can access it at any time by pressing \"TAB\".\nPress \"Escape\" to quit the DataBase.";
        }
        if(spike2 == true)
        {
            spikeName.text = "Mimitìs akantodis";
            spikeImage.enabled = true;
        }
        if(spike3 == true)
        {
            spikeLore.text = "This little animal has evolved to look like plants in its environment. This allows him to hide himself from big predators and surprise his prey.\n\nWe must not trust his appearance that can be misleading, he is carnivorous and feeds on small animals. He throws spades that grow on his forelimb, which allows him to reach prey that would flee too quickly for him if he approached.";
        }

        if(liana1 == false)
        {
            foreach(Text text in lianaButtons)
            {
                text.text = "???";
            }
            lianaTitle.text = "???";
            lianaName.text = "???";
            lianaImage.enabled = false;
            lianaWR.text = "???";
            lianaLore.text = "???";
        }
        if(liana1 == true)
        {
            foreach(Text text in lianaButtons)
            {
                text.text = "Parasite liana";
            }
            lianaTitle.text = "Parasite liana";
            lianaWR.text = "The body of this creature allows it to resist bullets because its body regenerates too quickly.\n\nThe only way to beat him is to cut his creepers with a sharp weapon. The creature is also very sensitive and will be momentarily paralyzed if we cut one of its lianas.";
        }
        if(liana2 == true)
        {
            lianaName.text = "Liana parasito";
            lianaImage.enabled = true;
        }
        if(liana3 == true)
        {
            lianaLore.text = "A parasitic plant has pushed into the body of an animal to take control of it.\nThe plant needs its host to move and feed, because only it will wither. It can not perfectly control the body it parasitizes. The parasitic plant uses its abilities to the detriment of the body it occupies.";
        }

        if(healer1 == false)
        {
            foreach(Text text in healerButtons)
            {
                text.text = "???";
            }
            healerTitle.text = "???";
            healerName.text = "???";
            healerImage.enabled = false;
            healerWR.text = "???";
            healerLore.text = "???";
        }
        if(healer1 == true)
        {
            foreach(Text text in healerButtons)
            {
                text.text = "Healing spore";
            }
            healerTitle.text = "Healing spore";
            healerWR.text = "We can often find this creature with other animals.\nIt can heal them in return of a protection.\nIt can also use those spores like a miracle fertilizer to help plants to grow";
        }
        if(healer2 == true)
        {
            healerName.text = "Therapeftís spòrio";
            healerImage.enabled = true;
        }
        if(healer3 == true)
        {
            healerLore.text = "Small harmless creature depend of the others animals. They hunt for it and it heal them with some spores.";
        }

        if(rock1 == false)
        {
            foreach(Text text in rockButtons)
            {
                text.text = "???";
            }
            rockTitle.text = "???";
            rockName.text = "???";
            rockImage.enabled = false;
            rockWR.text = "???";
            rockLore.text = "???";
        }
        if(rock1 == true)
        {
            foreach(Text text in rockButtons)
            {
                text.text = "Guardian colony";
            }
            rockTitle.text = "Guardian colony";
            rockWR.text = "This giant stone is actually an insect colony that has recovered a lot of materials to create a kind of mobile fortress.\nThe only way to defeat this enemy is to attack from the back of the monster, where the queen is uncovered.";
        }
        if(rock2 == true)
        {
            rockName.text = "Apoikia kidemónas";
            rockImage.enabled = true;
        }
        if(rock3 == true)
        {
            rockLore.text = "Apoikia Kidemónas are a species of insects living in colonies. They have developed an unprecedented survival technique. Their hive is made up of several types of debris that they stick with a sticky substance that they secrete.\n\nThe peculiarity of this hive is that it is moving and directed by the swarm itself. It uses it to defend itself. The shape can vary according to the living beings that they crossed before forming the nest.";
        }

        if(villager1 == false && villager2 == false && villager3 == false && villager4 == false && villager5 == false)
        {
            foreach(Text text in nativesButtons)
            {
                text.text = "???";
            }
            nativesTitle.text = "???";
            nativesImage.enabled = false;
            nativesChief.text = "???";
            nativesDrawings.text = "???";
            nativesLanguage.text = "???";
            nativesLore.text = "???";
            nativesTower.text = "???";
            nativesField.text = "???";
        }
        if(villager1 == true || villager2 == true || villager3 == true || villager4 == true && villager5 == false)
        {
            foreach(Text text in nativesButtons)
            {
                text.text = "Natives";
            }
            nativesTitle.text = "Natives";
            nativesImage.enabled = true;
        }
        if(villagerLanguage == true)
        {
            nativesLanguage.text = "To communicate, they use a very simple language. They use only a few simple words to express their ideas. They will not do any frills in their way of speaking and will get right to the point. Tip: They do not really understand our humor and even less the irony.";
        }
        if(villagerChief == true)
        {
            nativesChief.text = "A semblance of hierarchy is in place, based on a gerontocracy where decisions are made by a \"council of elders\".";
        }
        if(villagerDrawings == true)
        {
            nativesDrawings.text = "They do not know how to write. Therefore, they use different pigments to leave visual indications. They use it especially to remember exploits or to prevent the whole village, as of a great danger for example.";
        }
        if(villagerTower == true)
        {
            nativesTower.text = "They seem to have developed a proto-religion. In the village visited by the explorer Mira, the cult revolves around a sort of giant antenna, which did not have to be built by them. We can therefore assume that the object of their prayers may vary according to the villages.";
        }
        if(villagerField == true)
        {
            nativesField.text = "Their lifestyle is very self-sufficient, we can see some rare cases of barter, but they seem to share everything.\nThey grow few plants and hunt around their village. Having inherited the abilities of the plants, they also \"feed\" on the sun and therefore have a smaller conventional diet.\nThey draw most of the nutrients they need from the water points where they base their village.";
        }

        if(boss1 == false)
        {
            foreach(Text text in bossButtons)
            {
                text.text = "???";
            }
            bossTitle.text = "???";
            bossName.text = "???";
            bossImage.enabled = false;
            bossWR.text = "???";
            bossLore.text = "???";
            bossEnd.text = "???";
        }
        if(boss1 == true)
        {
            foreach(Text text in bossButtons)
            {
                text.text = "The flight of fire";
            }
            bossTitle.text = "The flight of fire";
            bossWR.text = "During its growth, the creature developed an extremely resistant skin, strong enough to withstand the claws and fangs of predators. But even if its skin is strong, she can not withstand big shocks.\n\nIn addition, if Ptìsi folia uses insects that ignite to attack, the contact of a tough projectile with flaming insects can cause a violent explosion to injure the animal.";
        }
        if(boss2 == true)
        {
            bossName.text = "Ptìsi folia";
            bossImage.enabled = true;
        }
        if(boss3 == true)
        {
            bossLore.text = "According to the inhabitants of the village, it is an extremely rare species. It grew up in an extremely rich and predator-free environment, which allowed it to grow.";
        }
        if(boss4 == true)
        {
            bossEnd.text = "When it reaches its adult height, he is a mini ecosystem on his own. A species of beetle lives between its feathers and feeds on everything that the pores of Ptìsi folia release.\n\nThese very special little insects can catch fire if they come in contact with a certain vitamin, which the beast can secrete on command. The Flight of Fire uses it in different ways to defend or hunt.";
        }
    }
}
