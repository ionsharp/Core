using System;

namespace Imagin.Core.Time;

[Serializable]
public enum TimeZone
{
    [DisplayName("(UTC-12:00) International Date Line West")]
    DatelineStandardTime,
    [DisplayName("(UTC-11:00) Coordinated Universal Time-11")]
    UTC11,
    [DisplayName("(UTC-10:00) Aleutian Islands")]
    AleutianStandardTime,
    [DisplayName("(UTC-10:00) Hawaii")]
    HawaiianStandardTime,
    [DisplayName("(UTC-09:30) Marquesas Islands")]
    MarquesasStandardTime,
    [DisplayName("(UTC-09:00) Alaska")]
    AlaskanStandardTime,
    [DisplayName("(UTC-09:00) Coordinated Universal Time-09")]
    UTC09,
    [DisplayName("(UTC-08:00) Baja California")]
    PacificStandardTimeMexico,
    [DisplayName("(UTC-08:00) Coordinated Universal Time-08")]
    UTC08,
    [DisplayName("(UTC-08:00) Pacific Time (US & Canada)")]
    PacificStandardTime,
    [DisplayName("(UTC-07:00) Arizona")]
    USMountainStandardTime,
    [DisplayName("(UTC-07:00) Chihuahua, La Paz, Mazatlan")]
    MountainStandardTimeMexico,
    [DisplayName("(UTC-07:00) Mountain Time (US & Canada)")]
    MountainStandardTime,
    [DisplayName("(UTC-06:00) Central America")]
    CentralAmericaStandardTime,
    [DisplayName("(UTC-06:00) Central Time (US & Canada)")]
    CentralStandardTime,
    [DisplayName("(UTC-06:00) Easter Island")]
    EasterIslandStandardTime,
    [DisplayName("(UTC-06:00) Guadalajara, Mexico City, Monterrey")]
    CentralStandardTimeMexico,
    [DisplayName("(UTC-06:00) Saskatchewan")]
    CanadaCentralStandardTime,
    [DisplayName("(UTC-05:00) Bogota, Lima, Quito, Rio Branco")]
    SAPacificStandardTime,
    [DisplayName("(UTC-05:00) Chetumal")]
    EasternStandardTimeMexico,
    [DisplayName("(UTC-05:00) Eastern Time (US & Canada)")]
    EasternStandardTime,
    [DisplayName("(UTC-05:00) Haiti")]
    HaitiStandardTime,
    [DisplayName("(UTC-05:00) Havana")]
    CubaStandardTime,
    [DisplayName("(UTC-05:00) Indiana (East)")]
    USEasternStandardTime,
    [DisplayName("(UTC-04:00) Asuncion")]
    ParaguayStandardTime,
    [DisplayName("(UTC-04:00) Atlantic Time (Canada)")]
    AtlanticStandardTime,
    [DisplayName("(UTC-04:00) Caracas")]
    VenezuelaStandardTime,
    [DisplayName("(UTC-04:00) Cuiaba")]
    CentralBrazilianStandardTime,
    [DisplayName("(UTC-04:00) Georgetown, La Paz, Manaus, San Juan")]
    SAWesternStandardTime,
    [DisplayName("(UTC-04:00) Santiago")]
    PacificSAStandardTime,
    [DisplayName("(UTC-04:00) Turks and Caicos")]
    TurksAndCaicosStandardTime,
    [DisplayName("(UTC-03:30) Newfoundland")]
    NewfoundlandStandardTime,
    [DisplayName("(UTC-03:00) Araguaina")]
    TocantinsStandardTime,
    [DisplayName("(UTC-03:00) Brasilia")]
    ESouthAmericaStandardTime,
    [DisplayName("(UTC-03:00) Cayenne, Fortaleza")]
    SAEasternStandardTime,
    [DisplayName("(UTC-03:00) City of Buenos Aires")]
    ArgentinaStandardTime,
    [DisplayName("(UTC-03:00) Greenland")]
    GreenlandStandardTime,
    [DisplayName("(UTC-03:00) Montevideo")]
    MontevideoStandardTime,
    [DisplayName("(UTC-03:00) Saint Pierre and Miquelon")]
    SaintPierreStandardTime,
    [DisplayName("(UTC-03:00) Salvador")]
    BahiaStandardTime,
    [DisplayName("(UTC-02:00) Coordinated Universal Time-02")]
    UTC02,
    [DisplayName("(UTC-02:00) Mid-Atlantic - Old")]
    MidAtlanticStandardTime,
    [DisplayName("(UTC-01:00) Azores")]
    AzoresStandardTime,
    [DisplayName("(UTC-01:00) Cabo Verde Is.")]
    CapeVerdeStandardTime,
    [DisplayName("(UTC) Coordinated Universal Time")]
    UTC,
    [DisplayName("(UTC+00:00) Casablanca")]
    MoroccoStandardTime,
    [DisplayName("(UTC+00:00) Dublin, Edinburgh, Lisbon, London")]
    GMTStandardTime,
    [DisplayName("(UTC+00:00) Monrovia, Reykjavik")]
    GreenwichStandardTime,
    [DisplayName("(UTC+01:00) Amsterdam, Berlin, Bern, Rome, Stockholm, Vienna")]
    WEuropeStandardTime,
    [DisplayName("(UTC+01:00) Belgrade, Bratislava, Budapest, Ljubljana, Prague")]
    CentralEuropeStandardTime,
    [DisplayName("(UTC+01:00) Brussels, Copenhagen, Madrid, Paris")]
    RomanceStandardTime,
    [DisplayName("(UTC+01:00) Sarajevo, Skopje, Warsaw, Zagreb")]
    CentralEuropeanStandardTime,
    [DisplayName("(UTC+01:00) West Central Africa")]
    WCentralAfricaStandardTime,
    [DisplayName("(UTC+01:00) Windhoek")]
    NamibiaStandardTime,
    [DisplayName("(UTC+02:00) Amman")]
    JordanStandardTime,
    [DisplayName("(UTC+02:00) Athens, Bucharest")]
    GTBStandardTime,
    [DisplayName("(UTC+02:00) Beirut")]
    MiddleEastStandardTime,
    [DisplayName("(UTC+02:00) Cairo")]
    EgyptStandardTime,
    [DisplayName("(UTC+02:00) Chisinau")]
    EEuropeStandardTime,
    [DisplayName("(UTC+02:00) Damascus")]
    SyriaStandardTime,
    [DisplayName("(UTC+02:00) Gaza, Hebron")]
    WestBankStandardTime,
    [DisplayName("(UTC+02:00) Harare, Pretoria")]
    SouthAfricaStandardTime,
    [DisplayName("(UTC+02:00) Helsinki, Kyiv, Riga, Sofia, Tallinn, Vilnius")]
    FLEStandardTime,
    [DisplayName("(UTC+02:00) Istanbul")]
    TurkeyStandardTime,
    [DisplayName("(UTC+02:00) Jerusalem")]
    IsraelStandardTime,
    [DisplayName("(UTC+02:00) Kaliningrad")]
    KaliningradStandardTime,
    [DisplayName("(UTC+02:00) Tripoli")]
    LibyaStandardTime,
    [DisplayName("(UTC+03:00) Baghdad")]
    ArabicStandardTime,
    [DisplayName("(UTC+03:00) Kuwait, Riyadh")]
    ArabStandardTime,
    [DisplayName("(UTC+03:00) Minsk")]
    BelarusStandardTime,
    [DisplayName("(UTC+03:00) Moscow, St. Petersburg, Volgograd")]
    RussianStandardTime,
    [DisplayName("(UTC+03:00) Nairobi")]
    EAfricaStandardTime,
    [DisplayName("(UTC+03:30) Tehran")]
    IranStandardTime,
    [DisplayName("(UTC+04:00) Abu Dhabi, Muscat")]
    ArabianStandardTime,
    [DisplayName("(UTC+04:00) Astrakhan, Ulyanovsk")]
    AstrakhanStandardTime,
    [DisplayName("(UTC+04:00) Baku")]
    AzerbaijanStandardTime,
    [DisplayName("(UTC+04:00) Izhevsk, Samara")]
    RussiaTimeZone3,
    [DisplayName("(UTC+04:00) Port Louis")]
    MauritiusStandardTime,
    [DisplayName("(UTC+04:00) Tbilisi")]
    GeorgianStandardTime,
    [DisplayName("(UTC+04:00) Yerevan")]
    CaucasusStandardTime,
    [DisplayName("(UTC+04:30) Kabul")]
    AfghanistanStandardTime,
    [DisplayName("(UTC+05:00) Ashgabat, Tashkent")]
    WestAsiaStandardTime,
    [DisplayName("(UTC+05:00) Ekaterinburg")]
    EkaterinburgStandardTime,
    [DisplayName("(UTC+05:00) Islamabad, Karachi")]
    PakistanStandardTime,
    [DisplayName("(UTC+05:30) Chennai, Kolkata, Mumbai, New Delhi")]
    IndiaStandardTime,
    [DisplayName("(UTC+05:30) Sri Jayawardenepura")]
    SriLankaStandardTime,
    [DisplayName("(UTC+05:45) Kathmandu")]
    NepalStandardTime,
    [DisplayName("(UTC+06:00) Astana")]
    CentralAsiaStandardTime,
    [DisplayName("(UTC+06:00) Dhaka")]
    BangladeshStandardTime,
    [DisplayName("(UTC+06:00) Novosibirsk")]
    NCentralAsiaStandardTime,
    [DisplayName("(UTC+06:30) Yangon (Rangoon)")]
    MyanmarStandardTime,
    [DisplayName("(UTC+07:00) Bangkok, Hanoi, Jakarta")]
    SEAsiaStandardTime,
    [DisplayName("(UTC+07:00) Barnaul, Gorno-Altaysk")]
    AltaiStandardTime,
    [DisplayName("(UTC+07:00) Hovd")]
    WMongoliaStandardTime,
    [DisplayName("(UTC+07:00) Krasnoyarsk")]
    NorthAsiaStandardTime,
    [DisplayName("(UTC+07:00) Tomsk")]
    TomskStandardTime,
    [DisplayName("(UTC+08:00) Beijing, Chongqing, Hong Kong, Urumqi")]
    ChinaStandardTime,
    [DisplayName("(UTC+08:00) Irkutsk")]
    NorthAsiaEastStandardTime,
    [DisplayName("(UTC+08:00) Kuala Lumpur, Singapore")]
    SingaporeStandardTime,
    [DisplayName("(UTC+08:00) Perth")]
    WAustraliaStandardTime,
    [DisplayName("(UTC+08:00) Taipei")]
    TaipeiStandardTime,
    [DisplayName("(UTC+08:00) Ulaanbaatar")]
    UlaanbaatarStandardTime,
    [DisplayName("(UTC+08:30) Pyongyang")]
    NorthKoreaStandardTime,
    [DisplayName("(UTC+08:45) Eucla")]
    AusCentralWStandardTime,
    [DisplayName("(UTC+09:00) Chita")]
    TransbaikalStandardTime,
    [DisplayName("(UTC+09:00) Osaka, Sapporo, Tokyo")]
    TokyoStandardTime,
    [DisplayName("(UTC+09:00) Seoul")]
    KoreaStandardTime,
    [DisplayName("(UTC+09:00) Yakutsk")]
    YakutskStandardTime,
    [DisplayName("(UTC+09:30) Adelaide")]
    CenAustraliaStandardTime,
    [DisplayName("(UTC+09:30) Darwin")]
    AUSCentralStandardTime,
    [DisplayName("(UTC+10:00) Brisbane")]
    EAustraliaStandardTime,
    [DisplayName("(UTC+10:00) Canberra, Melbourne, Sydney")]
    AUSEasternStandardTime,
    [DisplayName("(UTC+10:00) Guam, Port Moresby")]
    WestPacificStandardTime,
    [DisplayName("(UTC+10:00) Hobart")]
    TasmaniaStandardTime,
    [DisplayName("(UTC+10:00) Vladivostok")]
    VladivostokStandardTime,
    [DisplayName("(UTC+10:30) Lord Howe Island")]
    LordHoweStandardTime,
    [DisplayName("(UTC+11:00) Bougainville Island")]
    BougainvilleStandardTime,
    [DisplayName("(UTC+11:00) Chokurdakh")]
    RussiaTimeZone10,
    [DisplayName("(UTC+11:00) Magadan")]
    MagadanStandardTime,
    [DisplayName("(UTC+11:00) Norfolk Island")]
    NorfolkStandardTime,
    [DisplayName("(UTC+11:00) Sakhalin")]
    SakhalinStandardTime,
    [DisplayName("(UTC+11:00) Solomon Is., New Caledonia")]
    CentralPacificStandardTime,
    [DisplayName("(UTC+12:00) Anadyr, Petropavlovsk-Kamchatsky")]
    RussiaTimeZone11,
    [DisplayName("(UTC+12:00) Auckland, Wellington")]
    NewZealandStandardTime,
    [DisplayName("(UTC+12:00) Coordinated Universal Time+12")]
    UTC12,
    [DisplayName("(UTC+12:00) Fiji")]
    FijiStandardTime,
    [DisplayName("(UTC+12:00) Petropavlovsk-Kamchatsky - Old")]
    KamchatkaStandardTime,
    [DisplayName("(UTC+12:45) Chatham Islands")]
    ChathamIslandsStandardTime,
    [DisplayName("(UTC+13:00) Nuku'alofa")]
    TongaStandardTime,
    [DisplayName("(UTC+13:00) Samoa")]
    SamoaStandardTime,
    [DisplayName("(UTC+14:00) Kiritimati Island")]
    LineIslandsStandardTime,
}