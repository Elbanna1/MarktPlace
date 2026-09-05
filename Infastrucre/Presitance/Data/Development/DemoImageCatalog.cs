namespace Persistence.Data.Development;

internal static class DemoImageCatalog
{
    public const string UserAgent =
        "MarkatPlaceDevSeeder/1.0 (+https://markatplace.local; development demo data)";

    internal sealed record Topic(string Folder, IReadOnlyList<string> Sources);

    public static class Names
    {
        public const string CarHatchback = "CarHatchback";
        public const string CarInterior = "CarInterior";
        public const string CarPickup = "CarPickup";
        public const string CarSedan = "CarSedan";
        public const string CarSuv = "CarSuv";
        public const string CarTaxi = "CarTaxi";
        public const string CraftsmanCarpenter = "CraftsmanCarpenter";
        public const string CraftsmanElectrician = "CraftsmanElectrician";
        public const string CraftsmanPainter = "CraftsmanPainter";
        public const string CraftsmanPlumber = "CraftsmanPlumber";
        public const string CraftsmanTechnician = "CraftsmanTechnician";
        public const string CraftsmanWelder = "CraftsmanWelder";
        public const string HeavyEquipment = "HeavyEquipment";
        public const string HeavyEquipmentLoader = "HeavyEquipmentLoader";
        public const string ItemBag = "ItemBag";
        public const string ItemHeadphones = "ItemHeadphones";
        public const string ItemIdCard = "ItemIdCard";
        public const string ItemKeys = "ItemKeys";
        public const string ItemLaptop = "ItemLaptop";
        public const string ItemPhone = "ItemPhone";
        public const string ItemWallet = "ItemWallet";
        public const string ItemWatch = "ItemWatch";
        public const string Motorcycle = "Motorcycle";
        public const string WorkshopAluminium = "WorkshopAluminium";
        public const string WorkshopCarpentry = "WorkshopCarpentry";
        public const string WorkshopCarRepair = "WorkshopCarRepair";
        public const string WorkshopElectrical = "WorkshopElectrical";
        public const string WorkshopGlass = "WorkshopGlass";
        public const string WorkshopKitchen = "WorkshopKitchen";
        public const string WorkshopMarble = "WorkshopMarble";
        public const string WorkshopPlumbing = "WorkshopPlumbing";
        public const string WorkshopWelding = "WorkshopWelding";
        public const string AvatarMale = "AvatarMale";
        public const string AvatarFemale = "AvatarFemale";
    }

    public static readonly IReadOnlyDictionary<string, Topic> Topics = new Dictionary<string, Topic>
    {
        [Names.CarHatchback] = new("ads", new[]
        {
            "https://upload.wikimedia.org/wikipedia/commons/thumb/7/7b/2007_Holden_Astra_%28AH_MY07.5%29_CD_3-door_hatchback_%282018-10-30%29_02.jpg/1280px-2007_Holden_Astra_%28AH_MY07.5%29_CD_3-door_hatchback_%282018-10-30%29_02.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/d/d0/2019_Mini_Cooper_Exclusive_1.5_Rear.jpg/1280px-2019_Mini_Cooper_Exclusive_1.5_Rear.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/9/9f/A_hatchback_car_at_a_filling_station_in_Ghana.jpg/1280px-A_hatchback_car_at_a_filling_station_in_Ghana.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/7/72/BMW_i3s%2C_IAA_2017%2C_Frankfurt_%281Y7A3292%29.jpg/1280px-BMW_i3s%2C_IAA_2017%2C_Frankfurt_%281Y7A3292%29.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/f/f5/Honda_CIVIC_HATCHBACK_%28DBA-FK7%29_used_as_a_Medical_Car_of_Suzuka_Circuit.jpg/1280px-Honda_CIVIC_HATCHBACK_%28DBA-FK7%29_used_as_a_Medical_Car_of_Suzuka_Circuit.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/3/31/Honda_CIVIC_HATCHBACK_%28DBA-FK7%29_used_as_a_Medical_Car_of_Suzuka_Circuit_%281%29.jpg/1280px-Honda_CIVIC_HATCHBACK_%28DBA-FK7%29_used_as_a_Medical_Car_of_Suzuka_Circuit_%281%29.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/1/19/Honda_CIVIC_HATCHBACK_%28DBA-FK7%29_used_as_a_Medical_Car_of_Suzuka_Circuit_%282%29.jpg/1280px-Honda_CIVIC_HATCHBACK_%28DBA-FK7%29_used_as_a_Medical_Car_of_Suzuka_Circuit_%282%29.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/3/37/Honda_CIVIC_HATCHBACK_%28DBA-FK7%29_used_as_a_Medical_Car_of_Suzuka_Circuit_%283%29.jpg/1280px-Honda_CIVIC_HATCHBACK_%28DBA-FK7%29_used_as_a_Medical_Car_of_Suzuka_Circuit_%283%29.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/b/b1/Honda_CIVIC_HATCHBACK_%28DBA-FK7%29_used_as_a_Medical_Car_of_Suzuka_Circuit_%284%29.jpg/1280px-Honda_CIVIC_HATCHBACK_%28DBA-FK7%29_used_as_a_Medical_Car_of_Suzuka_Circuit_%284%29.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/e/e0/Honda_CIVIC_HATCHBACK_%28FK7%29_used_as_a_Medical_Car_of_Suzuka_Circuit.jpg/1280px-Honda_CIVIC_HATCHBACK_%28FK7%29_used_as_a_Medical_Car_of_Suzuka_Circuit.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/a/ac/Honda_CIVIC_HATCHBACK_%28FK7%29_used_as_a_Medical_Car_of_Suzuka_Circuit_%281%29.jpg/1280px-Honda_CIVIC_HATCHBACK_%28FK7%29_used_as_a_Medical_Car_of_Suzuka_Circuit_%281%29.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/e/e0/Hyundai%2C_Paris_Motor_Show_2018%2C_Paris_%281Y7A1446%29.jpg/1280px-Hyundai%2C_Paris_Motor_Show_2018%2C_Paris_%281Y7A1446%29.jpg",
        }),

        [Names.CarInterior] = new("ads", new[]
        {
            "https://upload.wikimedia.org/wikipedia/commons/thumb/4/4f/1951_Nash-Healey_luxury_sports_gran_turismo_car_at_Rambler_Ranch_4of6.jpg/1280px-1951_Nash-Healey_luxury_sports_gran_turismo_car_at_Rambler_Ranch_4of6.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/1/11/1965_Chevrolet_Impala_Caprice_%28Dashboard%29%3B_2017-02-09.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/2/24/1968_Camaro_Dashboard.jpg/1280px-1968_Camaro_Dashboard.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/6/67/1973_Ford_Mustang_Interior_Dashboard_%2851838912069%29.jpg/1280px-1973_Ford_Mustang_Interior_Dashboard_%2851838912069%29.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/8/8b/1985_Pontiac_Fiero_GT_Interior_Dashboard_%2851839256275%29.jpg/1280px-1985_Pontiac_Fiero_GT_Interior_Dashboard_%2851839256275%29.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/2/25/1985_Pontiac_Fiero_GT_Interior_Dashboard_%2851839326235%29.jpg/1280px-1985_Pontiac_Fiero_GT_Interior_Dashboard_%2851839326235%29.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/c/c4/1988_Cadillac_Allante_Interior_Dashboard_%2851837604127%29.jpg/1280px-1988_Cadillac_Allante_Interior_Dashboard_%2851837604127%29.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/e/ec/1988_Cadillac_Allante_Interior_Dashboard_%2851838910709%29.jpg/1280px-1988_Cadillac_Allante_Interior_Dashboard_%2851838910709%29.jpg",
        }),

        [Names.CarPickup] = new("ads", new[]
        {
            "https://upload.wikimedia.org/wikipedia/commons/thumb/a/ac/%2754_%28%3F%29_Chevrolet_3100_Pickup_in_Lower_Saxony_%282024%29.jpg/1280px-%2754_%28%3F%29_Chevrolet_3100_Pickup_in_Lower_Saxony_%282024%29.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/1/18/1938_Dodge_Brothers_pickup_truck_%2816598619396%29.jpg/1280px-1938_Dodge_Brothers_pickup_truck_%2816598619396%29.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/6/69/1964_Chevrolet_pickup_truck_Park-%26-Ride_U.S._Route_2_West_Danville_VT_May_2021.jpg/1280px-1964_Chevrolet_pickup_truck_Park-%26-Ride_U.S._Route_2_West_Danville_VT_May_2021.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/d/da/1964_Chevrolet_pickup_truck_Park-%26-Ride_U.S._Route_2_West_Danville_VT_May_2021_rear.jpg/1280px-1964_Chevrolet_pickup_truck_Park-%26-Ride_U.S._Route_2_West_Danville_VT_May_2021_rear.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/5/5d/2024_Downtown_West_Allis_Classic_Car_Show_52_%281937_Ford_pickup%29.jpg/1280px-2024_Downtown_West_Allis_Classic_Car_Show_52_%281937_Ford_pickup%29.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/a/a5/2024_Downtown_West_Allis_Classic_Car_Show_59_%281930_Ford_pickup%29.jpg/1280px-2024_Downtown_West_Allis_Classic_Car_Show_59_%281930_Ford_pickup%29.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/b/b3/A_pickup_truck_relocating_some_furniture.jpg/1280px-A_pickup_truck_relocating_some_furniture.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/4/4c/Abandoned_pickup_at_Kelvin_A._Lewis_farm_in_Creeds_3.jpg/1280px-Abandoned_pickup_at_Kelvin_A._Lewis_farm_in_Creeds_3.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/3/3e/Black_pickup_truck_in_birnin_kebbi.jpg/1280px-Black_pickup_truck_in_birnin_kebbi.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/5/57/Chairs_inside_a_Pickup_Truck%2C_Monastiraki_Square%2C_Athens%2C_Greece.jpg/1280px-Chairs_inside_a_Pickup_Truck%2C_Monastiraki_Square%2C_Athens%2C_Greece.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/3/31/Cheveep_-_Old_pickup_truck_on_Oak_Street%2C_New_Orleans%2C_1_May_2024.jpg/1280px-Cheveep_-_Old_pickup_truck_on_Oak_Street%2C_New_Orleans%2C_1_May_2024.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/6/67/Chevrolet_Pickup_Truck_Mod_Kulmbach_2018_P6170255.jpg/1280px-Chevrolet_Pickup_Truck_Mod_Kulmbach_2018_P6170255.jpg",
        }),

        [Names.CarSedan] = new("ads", new[]
        {
            "https://upload.wikimedia.org/wikipedia/commons/thumb/7/7d/1932_Hudson_8_four-door_sedan_with_dual_side_mounts_in_black_at_Doc%27s_meet_VA_3of7.jpg/1280px-1932_Hudson_8_four-door_sedan_with_dual_side_mounts_in_black_at_Doc%27s_meet_VA_3of7.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/2/2b/1932_Hudson_8_four-door_sedan_with_dual_side_mounts_in_black_at_Doc%27s_meet_VA_4of7.jpg/1280px-1932_Hudson_8_four-door_sedan_with_dual_side_mounts_in_black_at_Doc%27s_meet_VA_4of7.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/8/87/1949_Hudson_Commodore_8_four-door_sedan_at_Hershey_2019_AACA_show_2of7.jpg/1280px-1949_Hudson_Commodore_8_four-door_sedan_at_Hershey_2019_AACA_show_2of7.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/e/ec/1949_Nash_600_four-door_sedan_in_green_and_black_at_2017_Rockville_Maryland_show_01of16.jpg/1280px-1949_Nash_600_four-door_sedan_in_green_and_black_at_2017_Rockville_Maryland_show_01of16.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/2/2e/1949_Nash_600_four-door_sedan_in_green_and_black_at_2017_Rockville_Maryland_show_03of16.jpg/1280px-1949_Nash_600_four-door_sedan_in_green_and_black_at_2017_Rockville_Maryland_show_03of16.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/0/02/1949_Nash_600_four-door_sedan_in_green_and_black_at_2017_Rockville_Maryland_show_08of16.jpg/1280px-1949_Nash_600_four-door_sedan_in_green_and_black_at_2017_Rockville_Maryland_show_08of16.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/d/de/1953_Husdon_Hornet_four-door_sedan_two-tone_at_2021_Doc%27s_meet_Virginia_05of10.jpg/1280px-1953_Husdon_Hornet_four-door_sedan_two-tone_at_2021_Doc%27s_meet_Virginia_05of10.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/c/cf/1953_Husdon_Hornet_four-door_sedan_two-tone_at_2021_Doc%27s_meet_Virginia_06of10.jpg/1280px-1953_Husdon_Hornet_four-door_sedan_two-tone_at_2021_Doc%27s_meet_Virginia_06of10.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/8/8c/1954_Hudson_Hornet_Twin_H_sedan_green_ls.jpg/1280px-1954_Hudson_Hornet_Twin_H_sedan_green_ls.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/6/61/1954_Hudson_Jet_two-door_sedan_in_blue_and_white_at_2021_Doc%27s_meet_Virginia_3of5.jpg/1280px-1954_Hudson_Jet_two-door_sedan_in_blue_and_white_at_2021_Doc%27s_meet_Virginia_3of5.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/f/fa/1958_Continental_Mark_III_4-door_sedan%2C_left_side%2C_06-16-2024.jpg/1280px-1958_Continental_Mark_III_4-door_sedan%2C_left_side%2C_06-16-2024.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/c/cf/1958_Rambler_American_two-door_sedan_in_red_and_white_at_PA_meet_3of5.jpg/1280px-1958_Rambler_American_two-door_sedan_in_red_and_white_at_PA_meet_3of5.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/7/7f/1959_Edsel_Ranger_2-door_sedan%2C_right_side_%282022_Back_to_the_50%27s_Weekend%29.jpg/1280px-1959_Edsel_Ranger_2-door_sedan%2C_right_side_%282022_Back_to_the_50%27s_Weekend%29.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/c/ca/1962_Rambler_American_400_2-door_sedan_gold-white_at_2021_AMO_meet_05of15.jpg/1280px-1962_Rambler_American_400_2-door_sedan_gold-white_at_2021_AMO_meet_05of15.jpg",
        }),

        [Names.CarSuv] = new("ads", new[]
        {
            "https://upload.wikimedia.org/wikipedia/commons/thumb/3/36/00-06_Chevrolet_Tahoe.jpg/1280px-00-06_Chevrolet_Tahoe.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/c/c4/2004_Chevrolet_Tahoe_5.3_V8.jpg/1280px-2004_Chevrolet_Tahoe_5.3_V8.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/7/7b/2008_Chevrolet_Tahoe_%281%29.jpg/1280px-2008_Chevrolet_Tahoe_%281%29.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/a/a1/2008_Chevrolet_Tahoe_%282%29.jpg/1280px-2008_Chevrolet_Tahoe_%282%29.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/1/12/2008_Chevrolet_Tahoe_%283%29.jpg/1280px-2008_Chevrolet_Tahoe_%283%29.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/c/c3/2015_Chevrolet_Tahoe_LT_5.3L_front_3.24.19.jpg/1280px-2015_Chevrolet_Tahoe_LT_5.3L_front_3.24.19.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/9/91/2021_Chevrolet_Tahoe_at_the_2020_Canadian_International_Auto_Show.jpg/1280px-2021_Chevrolet_Tahoe_at_the_2020_Canadian_International_Auto_Show.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/5/54/2024_Chevrolet_Tahoe_5.3_High_Country_V8%2C_06-20-2024.jpg/1280px-2024_Chevrolet_Tahoe_5.3_High_Country_V8%2C_06-20-2024.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/d/db/2024_Chevrolet_Tahoe_5.3_Z71_V8%2C_front_right%2C_06-16-2024.jpg/1280px-2024_Chevrolet_Tahoe_5.3_Z71_V8%2C_front_right%2C_06-16-2024.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/f/f2/2024_Chevrolet_Tahoe_5.3_Z71_V8%2C_rear_right%2C_06-16-2024.jpg/1280px-2024_Chevrolet_Tahoe_5.3_Z71_V8%2C_rear_right%2C_06-16-2024.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/a/a7/CCSDPD_2015-20_Chevrolet_Tahoe_in_Starbucks_Drive-Thru.jpg/1280px-CCSDPD_2015-20_Chevrolet_Tahoe_in_Starbucks_Drive-Thru.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/a/a5/CCSDPD_Badge_on_Tahoe_-_LEAD_2020.jpg/1280px-CCSDPD_Badge_on_Tahoe_-_LEAD_2020.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/a/a2/CCSDPD_Chevrolet_Tahoe_on_S_Maryland_Pkwy.jpg/1280px-CCSDPD_Chevrolet_Tahoe_on_S_Maryland_Pkwy.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/0/00/CCSDPD_Tahoe_Door_%26_Fender_Decals.jpg/1280px-CCSDPD_Tahoe_Door_%26_Fender_Decals.jpg",
        }),

        [Names.CarTaxi] = new("ads", new[]
        {
            "https://upload.wikimedia.org/wikipedia/commons/thumb/f/f9/Anti-Uber_slogans_on_taxicab_at_Mexico_City_protest.jpg/1280px-Anti-Uber_slogans_on_taxicab_at_Mexico_City_protest.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/f/fe/Austin_FX4_taxicab%2C_London_%2818584985866%29.jpg/1280px-Austin_FX4_taxicab%2C_London_%2818584985866%29.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/d/d0/Ford_Escape_Hybrid_%28New_York_City_Taxicab%29.jpg/1280px-Ford_Escape_Hybrid_%28New_York_City_Taxicab%29.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/4/45/Human-powered_taxicab_in_Mexico_City%2C_March_2010.jpg/1280px-Human-powered_taxicab_in_Mexico_City%2C_March_2010.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/2/29/LTI_TX4_taxicab%2CLondon_%2821828163991%29.jpg/1280px-LTI_TX4_taxicab%2CLondon_%2821828163991%29.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/2/2d/Mumbai%2C_India%2C_Mumbai_taxi_%28taxicab%29.jpg/1280px-Mumbai%2C_India%2C_Mumbai_taxi_%28taxicab%29.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/f/f4/Mumbai%2C_India%2C_Mumbai_taxi_%28taxicab%29_on_the_road.jpg/1280px-Mumbai%2C_India%2C_Mumbai_taxi_%28taxicab%29_on_the_road.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/1/1b/New_York_City_yellow_taxicabs.jpg/1280px-New_York_City_yellow_taxicabs.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/8/8b/New_York_City_yellow_taxicabs_awaiting_dispatch.jpg/1280px-New_York_City_yellow_taxicabs_awaiting_dispatch.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/4/4b/New_York_City_yellow_taxicabs_in_an_airport.jpg/1280px-New_York_City_yellow_taxicabs_in_an_airport.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/b/b9/New_York_City_yellow_taxicabs_waiting.jpg/1280px-New_York_City_yellow_taxicabs_waiting.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/b/b6/Pre-facelift_Ford_Transit_Connect_New_York_City_yellow_taxicab.jpg/1280px-Pre-facelift_Ford_Transit_Connect_New_York_City_yellow_taxicab.jpg",
        }),

        [Names.CraftsmanCarpenter] = new("craftsmen", new[]
        {
            "https://upload.wikimedia.org/wikipedia/commons/9/92/%28John_Pydynkowski%29_made_this.._hand_carved._a_custom_made_to_order._estimated_to_have_%CC%A9175_u.s.d._cost_on_gold_leaf..._sold_for_%CC%A9350_u.s.d._Gold_Whale.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/0/0a/66789_Bow_Depot_to_Tonbridge_6O60_%2825838232027%29.jpg/1280px-66789_Bow_Depot_to_Tonbridge_6O60_%2825838232027%29.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/2/2b/Carpenter_at_working.jpg/1280px-Carpenter_at_working.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/1/10/Carpenter_bee_at_work.jpg/1280px-Carpenter_bee_at_work.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/7/7c/Carpentering.jpg/1280px-Carpentering.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/a/a2/Carpenters_Working_at_Ggaba_Landing_Site%2C_Uganda.jpg/1280px-Carpenters_Working_at_Ggaba_Landing_Site%2C_Uganda.jpg",
        }),

        [Names.CraftsmanElectrician] = new("craftsmen", new[]
        {
            "https://upload.wikimedia.org/wikipedia/commons/thumb/f/f5/Bound_to_be_an_Electrician_p010.jpg/1280px-Bound_to_be_an_Electrician_p010.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/b/be/Bound_to_be_an_Electrician_p025.jpg/1280px-Bound_to_be_an_Electrician_p025.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/3/3b/Bound_to_be_an_Electrician_p113.jpg/1280px-Bound_to_be_an_Electrician_p113.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/8/89/Car_Electrician_2.jpg/1280px-Car_Electrician_2.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/e/ea/Car_electrician_05.jpg/1280px-Car_electrician_05.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/a/a0/Car_electrician_4.jpg/1280px-Car_electrician_4.jpg",
        }),

        [Names.CraftsmanPainter] = new("craftsmen", new[]
        {
            "https://upload.wikimedia.org/wikipedia/commons/thumb/d/d5/0086_wall_painting.jpg/1280px-0086_wall_painting.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/e/e0/A_home_painter_at_work.jpg/1280px-A_home_painter_at_work.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/b/bb/Augustus_Wall_Callcott_%281779-1844%29_-_A_Mill_near_Llangollen_-_732269_-_National_Trust.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/b/b6/Augustus_Wall_Callcott_%281779-1844%29_-_Coast_Scene_-_WAG_193_-_Sudley_House.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/a/ac/Augustus_Wall_Callcott_%281779-1844%29_-_Landscape%2C_Market_Day_-_212.2_-_Tabley_House.jpg",
        }),

        [Names.CraftsmanPlumber] = new("craftsmen", new[]
        {
            "https://upload.wikimedia.org/wikipedia/commons/thumb/8/83/0617-vector-orange-man-pipe-wrench-01.jpg/1280px-0617-vector-orange-man-pipe-wrench-01.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/f/f0/492nd_Engineer_Company_soldiers_complete_detainee_holding_area_construction_140327-A-LO368-003.jpg/1280px-492nd_Engineer_Company_soldiers_complete_detainee_holding_area_construction_140327-A-LO368-003.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/8/86/500th_ESC_Builds_for_Future_Rotations_at_Saber_Junction_25_%289300522%29.jpg/1280px-500th_ESC_Builds_for_Future_Rotations_at_Saber_Junction_25_%289300522%29.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/f/f8/500th_ESC_Builds_for_Future_Rotations_at_Saber_Junction_25_%289300524%29.jpg/1280px-500th_ESC_Builds_for_Future_Rotations_at_Saber_Junction_25_%289300524%29.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/6/63/500th_ESC_Builds_for_Future_Rotations_at_Saber_Junction_25_%289300531%29.jpg/1280px-500th_ESC_Builds_for_Future_Rotations_at_Saber_Junction_25_%289300531%29.jpg",
        }),

        [Names.CraftsmanTechnician] = new("craftsmen", new[]
        {
            "https://upload.wikimedia.org/wikipedia/commons/thumb/2/25/A_phone_technician_at_work.jpg/1280px-A_phone_technician_at_work.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/d/d4/A_young_girl_repairing_phones.jpg/1280px-A_young_girl_repairing_phones.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/c/c8/Auto_mechanic_technician_fixing_a_malfunctioning_car.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/4/44/Camera_technician_1.jpg/1280px-Camera_technician_1.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/f/fd/Car_mechanic_technician_servicing_the_car_bonnet.jpg",
        }),

        [Names.CraftsmanWelder] = new("craftsmen", new[]
        {
            "https://upload.wikimedia.org/wikipedia/commons/thumb/2/24/%22Line_up_of_some_of_women_welders_including_the_women%27s_welding_champion_of_Ingalls_%28Shipbuilding_Corp.%2C_Pascagoula%2C_MS%29_-_NARA_-_522890.jpg/1280px-%22Line_up_of_some_of_women_welders_including_the_women%27s_welding_champion_of_Ingalls_%28Shipbuilding_Corp.%2C_Pascagoula%2C_MS%29_-_NARA_-_522890.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/7/7b/A_KIBBUTZ_MEMBER_WORKING_AS_A_WELDER_IN_THE_METAL_WORKSHOP_AT_KIBBUTZ_HAZOREA._%D7%97%D7%91%D7%A8%D7%AA_%D7%A7%D7%99%D7%91%D7%95%D7%A5_%D7%94%D7%96%D7%95%D7%A8%D7%A2_%D7%A2%D7%95%D7%91%D7%93%D7%AA_%D7%9B%D7%A8%D7%AA%D7%9B%D7%AA_%D7%91%D7%91%D7%99%D7%AA_%D7%94%D7%9E%D7%9C%D7%90%D7%9B%D7%94_%D7%A9%D7%9C_%D7%94%D7%A7%D7%99%D7%91%D7%95%D7%A5.D16-103.jpg/1280px-thumbnail.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/b/b0/A_man_working_as_a_seamstress.jpg/1280px-A_man_working_as_a_seamstress.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/e/ea/A_man_working_as_a_seamstress2.jpg/1280px-A_man_working_as_a_seamstress2.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/d/df/A_welder_at_work.jpg/1280px-A_welder_at_work.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/3/32/A_welder_working_on_Almat_farm.jpg/1280px-A_welder_working_on_Almat_farm.jpg",
        }),

        [Names.HeavyEquipment] = new("ads", new[]
        {
            "https://upload.wikimedia.org/wikipedia/commons/thumb/2/2d/2023-02-13_-_JCB_JS220LC_hydraulic_excavator_-_01.jpg/1280px-2023-02-13_-_JCB_JS220LC_hydraulic_excavator_-_01.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/8/85/2023-02-13_-_JCB_JS220LC_hydraulic_excavator_-_02.jpg/1280px-2023-02-13_-_JCB_JS220LC_hydraulic_excavator_-_02.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/1/18/2023-02-13_-_JCB_JS220LC_hydraulic_excavator_-_03.jpg/1280px-2023-02-13_-_JCB_JS220LC_hydraulic_excavator_-_03.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/2/26/Agricolankirkon_torni_kuvattuna_merelt%C3%A4.png/1280px-Agricolankirkon_torni_kuvattuna_merelt%C3%A4.png",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/a/af/Amphibious_excavator_in_a_swamp.jpg/1280px-Amphibious_excavator_in_a_swamp.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/f/fc/Amphibious_excavator_in_delta.jpg/1280px-Amphibious_excavator_in_delta.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/e/ef/An_excavator_at_work_in_the_expansive_construction_stone_quarry_of_Marurui_Roysambu%2C_Nairobi%2C_Kenya..jpg/1280px-An_excavator_at_work_in_the_expansive_construction_stone_quarry_of_Marurui_Roysambu%2C_Nairobi%2C_Kenya..jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/f/ff/Bobcat_E50_mini_excavator_with_oversized_treads.jpg/1280px-Bobcat_E50_mini_excavator_with_oversized_treads.jpg",
        }),

        [Names.HeavyEquipmentLoader] = new("ads", new[]
        {
            "https://upload.wikimedia.org/wikipedia/commons/thumb/e/e8/Atlas_52E_loader_2.jpg/1280px-Atlas_52E_loader_2.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/5/5f/CASE-Wheel-Loader.jpg/1280px-CASE-Wheel-Loader.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/9/96/CATERPILLAR_950_GC_Wheel_Loader_01.jpg/1280px-CATERPILLAR_950_GC_Wheel_Loader_01.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/6/69/CATERPILLAR_950_GC_Wheel_Loader_02.jpg/1280px-CATERPILLAR_950_GC_Wheel_Loader_02.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/e/e4/CATERPILLAR_950_GC_Wheel_Loader_03.jpg/1280px-CATERPILLAR_950_GC_Wheel_Loader_03.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/4/4f/CATERPILLAR_950_GC_Wheel_Loader_04.jpg/1280px-CATERPILLAR_950_GC_Wheel_Loader_04.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/1/1b/CATERPILLAR_950_GC_Wheel_Loader_05.jpg/1280px-CATERPILLAR_950_GC_Wheel_Loader_05.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/1/18/Caterpillar_950k_Wheel_Loader.jpg/1280px-Caterpillar_950k_Wheel_Loader.jpg",
        }),

        [Names.ItemBag] = new("lostfound", new[]
        {
            "https://upload.wikimedia.org/wikipedia/commons/thumb/d/df/ARVN_Rucksack_2.JPG/1280px-ARVN_Rucksack_2.JPG",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/0/0a/ARVN_Rucksack_3.JPG/1280px-ARVN_Rucksack_3.JPG",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/a/a3/A_backpack_with_trekking_poles_and_shoes.jpg/1280px-A_backpack_with_trekking_poles_and_shoes.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/b/ba/A_school_Bag.jpg/1280px-A_school_Bag.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/9/94/Aarn_Featherlite_Freedom_balance_pack_and_balance_pockets.jpg/1280px-Aarn_Featherlite_Freedom_balance_pack_and_balance_pockets.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/7/70/Adidas_bag%2C_frong.jpg/1280px-Adidas_bag%2C_frong.jpg",
        }),

        [Names.ItemHeadphones] = new("lostfound", new[]
        {
            "https://upload.wikimedia.org/wikipedia/commons/thumb/b/b8/%3DHeadphone.JPG/1280px-%3DHeadphone.JPG",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/1/1e/Audio-Technica_ATH-AD7_001.jpg/1280px-Audio-Technica_ATH-AD7_001.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/e/e4/Audio-Technica_ATH-AD7_002.jpg/1280px-Audio-Technica_ATH-AD7_002.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/7/7e/Audio-Technica_ATH-M70x_Headphones_folded.jpg/1280px-Audio-Technica_ATH-M70x_Headphones_folded.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/5/5a/Audio-Technica_Headphones_%28Custom_Earpads%29.jpg/1280px-Audio-Technica_Headphones_%28Custom_Earpads%29.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/1/1d/AudioQuest_NightHawk_Carbon_headphones_%2833582571334%29.jpg/1280px-AudioQuest_NightHawk_Carbon_headphones_%2833582571334%29.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/4/49/2014-12-21_02.09.27_plantronics_versatile.jpg/1280px-2014-12-21_02.09.27_plantronics_versatile.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/2/22/20240523_%EB%B8%8C%EB%A6%AC%EC%B8%A0_%EB%B8%94%EB%A3%A8%ED%88%AC%EC%8A%A4_%ED%97%A4%EB%93%9C%EC%85%8B.jpg/1280px-20240523_%EB%B8%8C%EB%A6%AC%EC%B8%A0_%EB%B8%94%EB%A3%A8%ED%88%AC%EC%8A%A4_%ED%97%A4%EB%93%9C%EC%85%8B.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/7/77/AERO_Friedrichshafen_2018%2C_Friedrichshafen_%281X7A4483%29.jpg/1280px-AERO_Friedrichshafen_2018%2C_Friedrichshafen_%281X7A4483%29.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/c/c0/Bluetooth_headset.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/2/24/Bluetooth_headset_WEP-200-8934.jpg/1280px-Bluetooth_headset_WEP-200-8934.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/a/ae/Cologne_Germany_Safety-helmet-with-headset-01.jpg/1280px-Cologne_Germany_Safety-helmet-with-headset-01.jpg",
        }),

        [Names.ItemIdCard] = new("lostfound", new[]
        {
            "https://upload.wikimedia.org/wikipedia/commons/8/88/Aaron_Bank_identity_card.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/2/28/California_fake_id_card_driver_license.jpg/1280px-California_fake_id_card_driver_license.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/b/b2/Estonian_Digital_Identity_Card_Issued_to_E-Resident_-_Issued_2022.jpg/1280px-Estonian_Digital_Identity_Card_Issued_to_E-Resident_-_Issued_2022.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/f/f7/Iranian_identity_card.jpg/1280px-Iranian_identity_card.jpg",
        }),

        [Names.ItemKeys] = new("lostfound", new[]
        {
            "https://upload.wikimedia.org/wikipedia/commons/thumb/0/0e/2020_Ford_key_fob_inside_look.jpg/1280px-2020_Ford_key_fob_inside_look.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/3/3a/221B_Baker_street_keychain.jpg/1280px-221B_Baker_street_keychain.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/a/ad/Alfa_Romeo_keychain_which_came_with_an_Alfa_Romeo_33_in_the_1980s%2C_without_keys.jpg/1280px-Alfa_Romeo_keychain_which_came_with_an_Alfa_Romeo_33_in_the_1980s%2C_without_keys.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/a/a2/Blumenbar-schluesselanhaenger-2022.jpg/1280px-Blumenbar-schluesselanhaenger-2022.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/7/7b/Hawaii_Rainbow_Warriors_Keychain.jpg/1280px-Hawaii_Rainbow_Warriors_Keychain.jpg",
        }),

        [Names.ItemLaptop] = new("lostfound", new[]
        {
            "https://upload.wikimedia.org/wikipedia/commons/thumb/d/de/2006_broken_laptop_computer_237398638.jpg/1280px-2006_broken_laptop_computer_237398638.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/f/f5/A_technical_laptop_repairer.jpg/1280px-A_technical_laptop_repairer.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/d/d9/Backlit_Laptop_Keyboard.jpg/1280px-Backlit_Laptop_Keyboard.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/b/b9/Camera_-_Desk_-_Computer_-_Notepad_-_Life_%28Unsplash%29.jpg/1280px-Camera_-_Desk_-_Computer_-_Notepad_-_Life_%28Unsplash%29.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/2/23/Clutterage.jpg/1280px-Clutterage.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/a/af/Computer_desk_Before.jpg/1280px-Computer_desk_Before.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/b/ba/DOTkamina_writing.png",
            "https://upload.wikimedia.org/wikipedia/commons/7/75/Desktop_workspace_in_transition_2008.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/7/74/Magnification_app_-_Sherm_for_Disabled_And_Here.png/1280px-Magnification_app_-_Sherm_for_Disabled_And_Here.png",
        }),

        [Names.ItemPhone] = new("lostfound", new[]
        {
            "https://upload.wikimedia.org/wikipedia/commons/thumb/c/c6/5_different_Smartphones.jpg/1280px-5_different_Smartphones.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/5/50/Back_of_Galaxy_Note_10%2C_Galaxy_Note_10%2B_and_Galaxy_Note_10_Lite_20200220a.jpg/1280px-Back_of_Galaxy_Note_10%2C_Galaxy_Note_10%2B_and_Galaxy_Note_10_Lite_20200220a.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/b/be/Blackview_A60_Smartphone_Android_mobile_phone_and_folio_case.jpg/1280px-Blackview_A60_Smartphone_Android_mobile_phone_and_folio_case.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/7/78/Blackview_A60_Smartphone_Android_mobile_phone_back_face.jpg/1280px-Blackview_A60_Smartphone_Android_mobile_phone_back_face.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/1/14/Blackview_A60_Smartphone_Android_mobile_phone_front_face_lock_screen.jpg/1280px-Blackview_A60_Smartphone_Android_mobile_phone_front_face_lock_screen.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/1/12/Blackview_A60_Smartphone_Android_mobile_phone_front_face_logged_in_screen.jpg/1280px-Blackview_A60_Smartphone_Android_mobile_phone_front_face_logged_in_screen.jpg",
        }),

        [Names.ItemWallet] = new("lostfound", new[]
        {
            "https://upload.wikimedia.org/wikipedia/commons/thumb/1/15/Aarong_leather_wallet.jpg/1280px-Aarong_leather_wallet.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/5/5f/Brown_Leather_Wallet_with_Embroidered_Decoration_-_DPLA_-_d7979a2b0d6da8fa8783bd0edf3c7a20_%28page_12%29.jpg/1280px-Brown_Leather_Wallet_with_Embroidered_Decoration_-_DPLA_-_d7979a2b0d6da8fa8783bd0edf3c7a20_%28page_12%29.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/3/34/Brown_Leather_Wallet_with_Embroidered_Decoration_-_DPLA_-_d7979a2b0d6da8fa8783bd0edf3c7a20_%28page_15%29.jpg/1280px-Brown_Leather_Wallet_with_Embroidered_Decoration_-_DPLA_-_d7979a2b0d6da8fa8783bd0edf3c7a20_%28page_15%29.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/3/33/Brown_Leather_Wallet_with_Embroidered_Decoration_-_DPLA_-_d7979a2b0d6da8fa8783bd0edf3c7a20_%28page_31%29.jpg/1280px-Brown_Leather_Wallet_with_Embroidered_Decoration_-_DPLA_-_d7979a2b0d6da8fa8783bd0edf3c7a20_%28page_31%29.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/f/ff/Brown_Leather_Wallet_with_Embroidered_Decoration_-_DPLA_-_d7979a2b0d6da8fa8783bd0edf3c7a20_%28page_5%29.jpg/1280px-Brown_Leather_Wallet_with_Embroidered_Decoration_-_DPLA_-_d7979a2b0d6da8fa8783bd0edf3c7a20_%28page_5%29.jpg",
        }),

        [Names.ItemWatch] = new("lostfound", new[]
        {
            "https://upload.wikimedia.org/wikipedia/commons/thumb/f/f8/Alexei_Leonov.jpg/1280px-Alexei_Leonov.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/c/c2/Ancre_Wristwatch_1970s_17_Jewels_Incabloc_%2851885919047%29.jpg/1280px-Ancre_Wristwatch_1970s_17_Jewels_Incabloc_%2851885919047%29.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/4/40/Corum_Coin_Watch-P5200012-black.jpg/1280px-Corum_Coin_Watch-P5200012-black.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/9/90/Counterfeit_Casio_F-91W_digital_watch.jpg/1280px-Counterfeit_Casio_F-91W_digital_watch.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/0/0c/Fashion-wristwatch-time-watch-1_%2824325510005%29.jpg/1280px-Fashion-wristwatch-time-watch-1_%2824325510005%29.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/0/04/Fashion-wristwatch-time-watch_%2824217032812%29.jpg/1280px-Fashion-wristwatch-time-watch_%2824217032812%29.jpg",
        }),

        [Names.Motorcycle] = new("ads", new[]
        {
            "https://upload.wikimedia.org/wikipedia/commons/thumb/c/cd/2009_Kawasaki_1400GTR_Kawasaki_Plaza_Akashi_Left_Side.jpg/1280px-2009_Kawasaki_1400GTR_Kawasaki_Plaza_Akashi_Left_Side.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/e/e2/2009_Kawasaki_1400GTR_Kawasaki_Plaza_Akashi_Right_Side.jpg/1280px-2009_Kawasaki_1400GTR_Kawasaki_Plaza_Akashi_Right_Side.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/f/f8/2010_Kawasaki_Concours_14_at_the_2009_Seattle_International_Motorcycle_Show_1.jpg/1280px-2010_Kawasaki_Concours_14_at_the_2009_Seattle_International_Motorcycle_Show_1.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/b/be/2022-09-24_Motorsport%2C_IDM%2C_Finale_Hockenheimring_1DX_3890_by_Stepro.jpg/1280px-2022-09-24_Motorsport%2C_IDM%2C_Finale_Hockenheimring_1DX_3890_by_Stepro.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/7/71/Bluroc_Motorcycle_Heritage_125_in_Westouter_%28Be%29_%282%29.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/4/4c/Boy_riding_a_motorcycle_in_Don_Det.jpg/1280px-Boy_riding_a_motorcycle_in_Don_Det.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/2/2a/Boy_riding_a_motorcycle_with_two_other_young_children_passengers%2C_in_a_street_of_Ban_Thateng_Sekong_Province_Laos.jpg/1280px-Boy_riding_a_motorcycle_with_two_other_young_children_passengers%2C_in_a_street_of_Ban_Thateng_Sekong_Province_Laos.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/4/41/Five_children_on_a_motorcycle.jpg/1280px-Five_children_on_a_motorcycle.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/f/fc/Indian_motorcycle_Main_Street_downtown_Montpelier_VT_August_2017.jpg/1280px-Indian_motorcycle_Main_Street_downtown_Montpelier_VT_August_2017.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/e/e4/Kawasaki-1400GTR_2007TMCS.jpg/1280px-Kawasaki-1400GTR_2007TMCS.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/e/e2/Kawasaki_1400GTR_grey_front_left_threequarter.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/5/5b/Kawasaki_1400GTR_grey_leftside.jpg/1280px-Kawasaki_1400GTR_grey_leftside.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/7/73/Kawasaki_1400GTR_grey_rear_threequarter.jpg/1280px-Kawasaki_1400GTR_grey_rear_threequarter.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/1/11/Kawasaki_1400GTR_left_rear_threequarter.jpg/1280px-Kawasaki_1400GTR_left_rear_threequarter.jpg",
        }),

        [Names.WorkshopAluminium] = new("workshops", new[]
        {
            "https://upload.wikimedia.org/wikipedia/commons/thumb/b/b2/Bishopsgate_London_EC2.jpg/1280px-Bishopsgate_London_EC2.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/4/4f/120808-A-JI408-006_%287758392298%29.jpg/1280px-120808-A-JI408-006_%287758392298%29.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/0/05/2017-03-01_Metal_fabrication_workshop%2C_Algoz.JPG/1280px-2017-03-01_Metal_fabrication_workshop%2C_Algoz.JPG",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/6/61/Contract_No._85%2C_Manufacture_and_Delivery_of_Precast_Concrete_Steel_Cylinder_Pipe%2C_Southborough%2C_Framingham%2C_Wayland%2C_Natick%2C_Weston%2C_bell_and_spigot_fabrication%2C_Lock_Joint_plant%2C_-_DPLA_-_17dcbecf9e1697b751df883ffa572fc6.jpg/1280px-thumbnail.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/d/d9/Gazette_des_beaux-arts_%281859%29_%2814803772323%29.jpg/1280px-Gazette_des_beaux-arts_%281859%29_%2814803772323%29.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/7/75/Juillet_1910%2C_usine_du_Bi-m%C3%A9tal_%28un_atelier_de_fabrication_de_casseroles_et_de_baquets%29_-_btv1b6914547h.jpg/1280px-Juillet_1910%2C_usine_du_Bi-m%C3%A9tal_%28un_atelier_de_fabrication_de_casseroles_et_de_baquets%29_-_btv1b6914547h.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/0/02/Juillet_1910%2C_usine_du_Bi-m%C3%A9tal_%28un_atelier_de_fabrication_de_grands_bacs_en_m%C3%A9tal%29_-_btv1b6914548x.jpg/1280px-Juillet_1910%2C_usine_du_Bi-m%C3%A9tal_%28un_atelier_de_fabrication_de_grands_bacs_en_m%C3%A9tal%29_-_btv1b6914548x.jpg",
        }),

        [Names.WorkshopCarpentry] = new("workshops", new[]
        {
            "https://upload.wikimedia.org/wikipedia/commons/thumb/1/12/Carpentry_shop_01_%E2%80%93_Panorama_%28Greg_Zaal_via_Poly_Haven%29.jpg/1280px-Carpentry_shop_01_%E2%80%93_Panorama_%28Greg_Zaal_via_Poly_Haven%29.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/2/24/Carpentry_shop_01_%E2%80%93_Preview_%28Greg_Zaal_via_Poly_Haven%29.png",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/d/d8/Carpentry_shop_02_%E2%80%93_Panorama_%28Greg_Zaal_via_Poly_Haven%29.jpg/1280px-Carpentry_shop_02_%E2%80%93_Panorama_%28Greg_Zaal_via_Poly_Haven%29.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/2/24/Carpentry_shop_02_%E2%80%93_Preview_%28Greg_Zaal_via_Poly_Haven%29.png",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/5/5a/Carpentry_workshop._Iran._Qom_city_%DA%A9%D8%A7%D8%B1%DA%AF%D8%A7%D9%87_%D9%86%D8%AC%D8%A7%D8%B1%DB%8C_%D8%A8%D8%B1%D8%A7%D8%AF%D8%B1%D8%A7%D9%86_%D8%AD%D8%A7%D8%AC_%D9%85%D8%AD%D9%85%D8%AF%DB%8C._%D8%A7%DB%8C%D8%B1%D8%A7%D9%86%D8%8C_%D9%82%D9%85_01.jpg/1280px-Carpentry_workshop._Iran._Qom_city_%DA%A9%D8%A7%D8%B1%DA%AF%D8%A7%D9%87_%D9%86%D8%AC%D8%A7%D8%B1%DB%8C_%D8%A8%D8%B1%D8%A7%D8%AF%D8%B1%D8%A7%D9%86_%D8%AD%D8%A7%D8%AC_%D9%85%D8%AD%D9%85%D8%AF%DB%8C._%D8%A7%DB%8C%D8%B1%D8%A7%D9%86%D8%8C_%D9%82%D9%85_01.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/f/f0/Carpentry_workshop._Iran._Qom_city_%DA%A9%D8%A7%D8%B1%DA%AF%D8%A7%D9%87_%D9%86%D8%AC%D8%A7%D8%B1%DB%8C_%D8%A8%D8%B1%D8%A7%D8%AF%D8%B1%D8%A7%D9%86_%D8%AD%D8%A7%D8%AC_%D9%85%D8%AD%D9%85%D8%AF%DB%8C._%D8%A7%DB%8C%D8%B1%D8%A7%D9%86%D8%8C_%D9%82%D9%85_02.jpg/1280px-Carpentry_workshop._Iran._Qom_city_%DA%A9%D8%A7%D8%B1%DA%AF%D8%A7%D9%87_%D9%86%D8%AC%D8%A7%D8%B1%DB%8C_%D8%A8%D8%B1%D8%A7%D8%AF%D8%B1%D8%A7%D9%86_%D8%AD%D8%A7%D8%AC_%D9%85%D8%AD%D9%85%D8%AF%DB%8C._%D8%A7%DB%8C%D8%B1%D8%A7%D9%86%D8%8C_%D9%82%D9%85_02.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/c/cc/Carpentry_workshop._Iran._Qom_city_%DA%A9%D8%A7%D8%B1%DA%AF%D8%A7%D9%87_%D9%86%D8%AC%D8%A7%D8%B1%DB%8C_%D8%A8%D8%B1%D8%A7%D8%AF%D8%B1%D8%A7%D9%86_%D8%AD%D8%A7%D8%AC_%D9%85%D8%AD%D9%85%D8%AF%DB%8C._%D8%A7%DB%8C%D8%B1%D8%A7%D9%86%D8%8C_%D9%82%D9%85_03.jpg/1280px-Carpentry_workshop._Iran._Qom_city_%DA%A9%D8%A7%D8%B1%DA%AF%D8%A7%D9%87_%D9%86%D8%AC%D8%A7%D8%B1%DB%8C_%D8%A8%D8%B1%D8%A7%D8%AF%D8%B1%D8%A7%D9%86_%D8%AD%D8%A7%D8%AC_%D9%85%D8%AD%D9%85%D8%AF%DB%8C._%D8%A7%DB%8C%D8%B1%D8%A7%D9%86%D8%8C_%D9%82%D9%85_03.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/b/b6/Carpentry_workshop._Iran._Qom_city_%DA%A9%D8%A7%D8%B1%DA%AF%D8%A7%D9%87_%D9%86%D8%AC%D8%A7%D8%B1%DB%8C_%D8%A8%D8%B1%D8%A7%D8%AF%D8%B1%D8%A7%D9%86_%D8%AD%D8%A7%D8%AC_%D9%85%D8%AD%D9%85%D8%AF%DB%8C._%D8%A7%DB%8C%D8%B1%D8%A7%D9%86%D8%8C_%D9%82%D9%85_04.jpg/1280px-Carpentry_workshop._Iran._Qom_city_%DA%A9%D8%A7%D8%B1%DA%AF%D8%A7%D9%87_%D9%86%D8%AC%D8%A7%D8%B1%DB%8C_%D8%A8%D8%B1%D8%A7%D8%AF%D8%B1%D8%A7%D9%86_%D8%AD%D8%A7%D8%AC_%D9%85%D8%AD%D9%85%D8%AF%DB%8C._%D8%A7%DB%8C%D8%B1%D8%A7%D9%86%D8%8C_%D9%82%D9%85_04.jpg",
        }),

        [Names.WorkshopCarRepair] = new("workshops", new[]
        {
            "https://upload.wikimedia.org/wikipedia/commons/thumb/6/6e/A_man_who_sanding_with_a_grinder_and_prepares_the_paint_for_the_car_in_a_home_service._-_Flickr_-_shixart1985.jpg/1280px-A_man_who_sanding_with_a_grinder_and_prepares_the_paint_for_the_car_in_a_home_service._-_Flickr_-_shixart1985.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/4/42/Car_mechanic_worker_repairing_suspension_of_lifted_automobile_at_auto_repair_garage_shop.jpg/1280px-Car_mechanic_worker_repairing_suspension_of_lifted_automobile_at_auto_repair_garage_shop.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/a/a1/Car_mechanic_worker_repairing_suspension_with_drill_in_garage.jpg/1280px-Car_mechanic_worker_repairing_suspension_with_drill_in_garage.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/c/c1/Mechanic_repairing_car_door_lock_on_concrete_surface_in_garage.jpg/1280px-Mechanic_repairing_car_door_lock_on_concrete_surface_in_garage.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/8/84/National_Drill_Shop_-_Vale%2C_Oregon_%2819846833504%29.jpg/1280px-National_Drill_Shop_-_Vale%2C_Oregon_%2819846833504%29.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/3/3d/National_Drill_Shop_-_Vale%2C_Oregon_%2819846839794%29.jpg/1280px-National_Drill_Shop_-_Vale%2C_Oregon_%2819846839794%29.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/2/2f/National_Drill_Shop_-_Vale%2C_Oregon_%2819848555313%29.jpg/1280px-National_Drill_Shop_-_Vale%2C_Oregon_%2819848555313%29.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/f/ff/National_Drill_Shop_-_Vale%2C_Oregon_%2820281352940%29.jpg/1280px-National_Drill_Shop_-_Vale%2C_Oregon_%2820281352940%29.jpg",
        }),

        [Names.WorkshopElectrical] = new("workshops", new[]
        {
            "https://upload.wikimedia.org/wikipedia/commons/thumb/9/93/20220127-RD-LSC-1007_%2851846460727%29.jpg/1280px-20220127-RD-LSC-1007_%2851846460727%29.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/9/9c/20220127-RD-LSC-1059_%2851848080775%29.jpg/1280px-20220127-RD-LSC-1059_%2851848080775%29.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/7/79/ELEC_Practical_Bays_Level_2_Wiring_Hands_On_Wolverhampton.jpg/1280px-ELEC_Practical_Bays_Level_2_Wiring_Hands_On_Wolverhampton.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/9/95/Electrical_Engineering_workshop.jpg/1280px-Electrical_Engineering_workshop.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/0/07/Electrical_wiring_Tobiashammer_Ohrdruf_001.JPG/1280px-Electrical_wiring_Tobiashammer_Ohrdruf_001.JPG",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/0/06/Electrical_wiring_Tobiashammer_Ohrdruf_002.JPG/1280px-Electrical_wiring_Tobiashammer_Ohrdruf_002.JPG",
        }),

        [Names.WorkshopGlass] = new("workshops", new[]
        {
            "https://upload.wikimedia.org/wikipedia/commons/thumb/2/29/BLW_Window_frame_and_stained_glass.jpg/1280px-BLW_Window_frame_and_stained_glass.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/0/07/Cathedral_Fribourg_vitrail_Georg_Michael_Anna_Maria_01.jpg/1280px-Cathedral_Fribourg_vitrail_Georg_Michael_Anna_Maria_01.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/d/d5/Glass%3B_a_glazier%27s_workshop_%28above%29%2C_the_tools_used_for_maki_Wellcome_V0024061EL.jpg/1280px-Glass%3B_a_glazier%27s_workshop_%28above%29%2C_the_tools_used_for_maki_Wellcome_V0024061EL.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/5/5c/Glazier_cutting_and_installing_glass_in_Northern_Nigeria_%2812%29.jpg/1280px-Glazier_cutting_and_installing_glass_in_Northern_Nigeria_%2812%29.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/a/a7/Great_East_Window_York_Minster.jpg/1280px-Great_East_Window_York_Minster.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/a/a8/Great_East_Window_York_Minster_-_51980814292.jpg/1280px-Great_East_Window_York_Minster_-_51980814292.jpg",
        }),

        [Names.WorkshopKitchen] = new("workshops", new[]
        {
            "https://upload.wikimedia.org/wikipedia/commons/thumb/7/72/Choson_dynasty_kitchen_cabinet%2C_V%26A_London_01.jpg/1280px-Choson_dynasty_kitchen_cabinet%2C_V%26A_London_01.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/d/d9/Choson_dynasty_kitchen_cabinet%2C_V%26A_London_02.jpg/1280px-Choson_dynasty_kitchen_cabinet%2C_V%26A_London_02.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/f/fa/Choson_dynasty_kitchen_cabinet%2C_V%26A_London_03.jpg/1280px-Choson_dynasty_kitchen_cabinet%2C_V%26A_London_03.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/f/f3/Dept_Agriculture_kitchen_cabinets_farmhouse_publication.jpg/1280px-Dept_Agriculture_kitchen_cabinets_farmhouse_publication.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/6/6b/Green_kitchen_cabinet_%281%29.jpg/1280px-Green_kitchen_cabinet_%281%29.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/e/eb/Green_kitchen_cabinet_%282%29.jpg/1280px-Green_kitchen_cabinet_%282%29.jpg",
        }),

        [Names.WorkshopMarble] = new("workshops", new[]
        {
            "https://upload.wikimedia.org/wikipedia/commons/thumb/5/56/MarmolesMaxei029.jpg/1280px-MarmolesMaxei029.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/c/c6/MarmolesMaxei032.jpg/1280px-MarmolesMaxei032.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/3/37/MarmolesMaxei033.jpg/1280px-MarmolesMaxei033.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/2/29/MarmolesMaxei034.jpg/1280px-MarmolesMaxei034.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/3/38/MarmolesMaxei035.jpg/1280px-MarmolesMaxei035.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/c/c6/MarmolesMaxei036.jpg/1280px-MarmolesMaxei036.jpg",
        }),

        [Names.WorkshopPlumbing] = new("workshops", new[]
        {
            "https://upload.wikimedia.org/wikipedia/commons/thumb/7/76/02022_0834_fittings_of_side_segments_of_type_II_helmets%2C_Khazar_Khaganate_-_%C3%81rp%C3%A1d_Period_%2810th%E2%80%9313th_centuries%29%2C_Szurpi%C5%82y.jpg/1280px-02022_0834_fittings_of_side_segments_of_type_II_helmets%2C_Khazar_Khaganate_-_%C3%81rp%C3%A1d_Period_%2810th%E2%80%9313th_centuries%29%2C_Szurpi%C5%82y.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/7/7b/020240817_130128_fittings_from_Lutomiersk.jpg/1280px-020240817_130128_fittings_from_Lutomiersk.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/9/9f/Adapter_SATA_mPCIe_IMGP1260_wp.jpg/1280px-Adapter_SATA_mPCIe_IMGP1260_wp.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/5/53/Adapter_f%C3%BCr_interne_HDD-Festplatten_Vorderseite_20210201_DSC7579.jpg/1280px-Adapter_f%C3%BCr_interne_HDD-Festplatten_Vorderseite_20210201_DSC7579.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/e/e4/Canon_Lens_Mount_Adapter_EF-EOS_M-M_mount-fs_PNr%C2%B00737.jpg/1280px-Canon_Lens_Mount_Adapter_EF-EOS_M-M_mount-fs_PNr%C2%B00737.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/d/d4/Canon_Lens_Mount_Adapter_EF-EOS_M-uncapped-back_oblique-fs_PNr%C2%B00735.jpg/1280px-Canon_Lens_Mount_Adapter_EF-EOS_M-uncapped-back_oblique-fs_PNr%C2%B00735.jpg",
        }),

        [Names.WorkshopWelding] = new("workshops", new[]
        {
            "https://upload.wikimedia.org/wikipedia/commons/thumb/b/b5/11th_CES_structures_foundation_for_Andrews_160601-F-IP635-102.jpg/1280px-11th_CES_structures_foundation_for_Andrews_160601-F-IP635-102.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/3/31/A_Ghanaian_metal_fabricator_welding_at_his_workshop.jpg/1280px-A_Ghanaian_metal_fabricator_welding_at_his_workshop.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/4/40/A_metal_welding_and_bending_shop.jpg/1280px-A_metal_welding_and_bending_shop.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/3/3d/A_wedge_on_a_newly_constructed_support_for_preassure_tanks_1.jpg/1280px-A_wedge_on_a_newly_constructed_support_for_preassure_tanks_1.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/a/a6/A_wedge_on_a_newly_constructed_support_for_preassure_tanks_2.jpg/1280px-A_wedge_on_a_newly_constructed_support_for_preassure_tanks_2.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/7/7e/A_wedge_on_a_newly_constructed_support_for_preassure_tanks_3.jpg/1280px-A_wedge_on_a_newly_constructed_support_for_preassure_tanks_3.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/8/8a/Andersen%27s_Blacksmithing_%28workshop_building%29%2C_Chico.jpg/1280px-Andersen%27s_Blacksmithing_%28workshop_building%29%2C_Chico.jpg",
            "https://upload.wikimedia.org/wikipedia/commons/thumb/c/cc/Damage_Controlman_2nd_Class_Kirstin_Kulus%2C_from_Wendell_Arizona%2C_uses_a_portable_exothermal_cutting_unit_to_cut_metal_in_the_general_workshop_aboard_the_USS_Stout_%28DDG-55%29.jpg/1280px-thumbnail.jpg",
        }),

        [Names.AvatarMale] = new("profile", Portraits("male", 20)),
        [Names.AvatarFemale] = new("profile", Portraits("female", 20))
    };

    private static string[] Portraits(string gender, int count) =>
        Enumerable.Range(0, count)
            .Select(index => $"https://xsgames.co/randomusers/assets/avatars/{gender}/{index}.jpg")
            .ToArray();
}
