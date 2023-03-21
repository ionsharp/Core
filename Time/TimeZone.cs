using System;

namespace Imagin.Core.Time;

[Serializable]
public enum TimeZone
{
    [Name("(UTC-12:00) International Date Line West")]
    DatelineStandardTime,
    [Name("(UTC-11:00) Coordinated Universal Time-11")]
    UTC11,
    [Name("(UTC-10:00) Aleutian Islands")]
    AleutianStandardTime,
    [Name("(UTC-10:00) Hawaii")]
    HawaiianStandardTime,
    [Name("(UTC-09:30) Marquesas Islands")]
    MarquesasStandardTime,
    [Name("(UTC-09:00) Alaska")]
    AlaskanStandardTime,
    [Name("(UTC-09:00) Coordinated Universal Time-09")]
    UTC09,
    [Name("(UTC-08:00) Baja California")]
    PacificStandardTimeMexico,
    [Name("(UTC-08:00) Coordinated Universal Time-08")]
    UTC08,
    [Name("(UTC-08:00) Pacific Time (US & Canada)")]
    PacificStandardTime,
    [Name("(UTC-07:00) Arizona")]
    USMountainStandardTime,
    [Name("(UTC-07:00) Chihuahua, La Paz, Mazatlan")]
    MountainStandardTimeMexico,
    [Name("(UTC-07:00) Mountain Time (US & Canada)")]
    MountainStandardTime,
    [Name("(UTC-06:00) Central America")]
    CentralAmericaStandardTime,
    [Name("(UTC-06:00) Central Time (US & Canada)")]
    CentralStandardTime,
    [Name("(UTC-06:00) Easter Island")]
    EasterIslandStandardTime,
    [Name("(UTC-06:00) Guadalajara, Mexico City, Monterrey")]
    CentralStandardTimeMexico,
    [Name("(UTC-06:00) Saskatchewan")]
    CanadaCentralStandardTime,
    [Name("(UTC-05:00) Bogota, Lima, Quito, Rio Branco")]
    SAPacificStandardTime,
    [Name("(UTC-05:00) Chetumal")]
    EasternStandardTimeMexico,
    [Name("(UTC-05:00) Eastern Time (US & Canada)")]
    EasternStandardTime,
    [Name("(UTC-05:00) Haiti")]
    HaitiStandardTime,
    [Name("(UTC-05:00) Havana")]
    CubaStandardTime,
    [Name("(UTC-05:00) Indiana (East)")]
    USEasternStandardTime,
    [Name("(UTC-04:00) Asuncion")]
    ParaguayStandardTime,
    [Name("(UTC-04:00) Atlantic Time (Canada)")]
    AtlanticStandardTime,
    [Name("(UTC-04:00) Caracas")]
    VenezuelaStandardTime,
    [Name("(UTC-04:00) Cuiaba")]
    CentralBrazilianStandardTime,
    [Name("(UTC-04:00) Georgetown, La Paz, Manaus, San Juan")]
    SAWesternStandardTime,
    [Name("(UTC-04:00) Santiago")]
    PacificSAStandardTime,
    [Name("(UTC-04:00) Turks and Caicos")]
    TurksAndCaicosStandardTime,
    [Name("(UTC-03:30) Newfoundland")]
    NewfoundlandStandardTime,
    [Name("(UTC-03:00) Araguaina")]
    TocantinsStandardTime,
    [Name("(UTC-03:00) Brasilia")]
    ESouthAmericaStandardTime,
    [Name("(UTC-03:00) Cayenne, Fortaleza")]
    SAEasternStandardTime,
    [Name("(UTC-03:00) City of Buenos Aires")]
    ArgentinaStandardTime,
    [Name("(UTC-03:00) Greenland")]
    GreenlandStandardTime,
    [Name("(UTC-03:00) Montevideo")]
    MontevideoStandardTime,
    [Name("(UTC-03:00) Saint Pierre and Miquelon")]
    SaintPierreStandardTime,
    [Name("(UTC-03:00) Salvador")]
    BahiaStandardTime,
    [Name("(UTC-02:00) Coordinated Universal Time-02")]
    UTC02,
    [Name("(UTC-02:00) Mid-Atlantic - Old")]
    MidAtlanticStandardTime,
    [Name("(UTC-01:00) Azores")]
    AzoresStandardTime,
    [Name("(UTC-01:00) Cabo Verde Is.")]
    CapeVerdeStandardTime,
    [Name("(UTC) Coordinated Universal Time")]
    UTC,
    [Name("(UTC+00:00) Casablanca")]
    MoroccoStandardTime,
    [Name("(UTC+00:00) Dublin, Edinburgh, Lisbon, London")]
    GMTStandardTime,
    [Name("(UTC+00:00) Monrovia, Reykjavik")]
    GreenwichStandardTime,
    [Name("(UTC+01:00) Amsterdam, Berlin, Bern, Rome, Stockholm, Vienna")]
    WEuropeStandardTime,
    [Name("(UTC+01:00) Belgrade, Bratislava, Budapest, Ljubljana, Prague")]
    CentralEuropeStandardTime,
    [Name("(UTC+01:00) Brussels, Copenhagen, Madrid, Paris")]
    RomanceStandardTime,
    [Name("(UTC+01:00) Sarajevo, Skopje, Warsaw, Zagreb")]
    CentralEuropeanStandardTime,
    [Name("(UTC+01:00) West Central Africa")]
    WCentralAfricaStandardTime,
    [Name("(UTC+01:00) Windhoek")]
    NamibiaStandardTime,
    [Name("(UTC+02:00) Amman")]
    JordanStandardTime,
    [Name("(UTC+02:00) Athens, Bucharest")]
    GTBStandardTime,
    [Name("(UTC+02:00) Beirut")]
    MiddleEastStandardTime,
    [Name("(UTC+02:00) Cairo")]
    EgyptStandardTime,
    [Name("(UTC+02:00) Chisinau")]
    EEuropeStandardTime,
    [Name("(UTC+02:00) Damascus")]
    SyriaStandardTime,
    [Name("(UTC+02:00) Gaza, Hebron")]
    WestBankStandardTime,
    [Name("(UTC+02:00) Harare, Pretoria")]
    SouthAfricaStandardTime,
    [Name("(UTC+02:00) Helsinki, Kyiv, Riga, Sofia, Tallinn, Vilnius")]
    FLEStandardTime,
    [Name("(UTC+02:00) Istanbul")]
    TurkeyStandardTime,
    [Name("(UTC+02:00) Jerusalem")]
    IsraelStandardTime,
    [Name("(UTC+02:00) Kaliningrad")]
    KaliningradStandardTime,
    [Name("(UTC+02:00) Tripoli")]
    LibyaStandardTime,
    [Name("(UTC+03:00) Baghdad")]
    ArabicStandardTime,
    [Name("(UTC+03:00) Kuwait, Riyadh")]
    ArabStandardTime,
    [Name("(UTC+03:00) Minsk")]
    BelarusStandardTime,
    [Name("(UTC+03:00) Moscow, St. Petersburg, Volgograd")]
    RussianStandardTime,
    [Name("(UTC+03:00) Nairobi")]
    EAfricaStandardTime,
    [Name("(UTC+03:30) Tehran")]
    IranStandardTime,
    [Name("(UTC+04:00) Abu Dhabi, Muscat")]
    ArabianStandardTime,
    [Name("(UTC+04:00) Astrakhan, Ulyanovsk")]
    AstrakhanStandardTime,
    [Name("(UTC+04:00) Baku")]
    AzerbaijanStandardTime,
    [Name("(UTC+04:00) Izhevsk, Samara")]
    RussiaTimeZone3,
    [Name("(UTC+04:00) Port Louis")]
    MauritiusStandardTime,
    [Name("(UTC+04:00) Tbilisi")]
    GeorgianStandardTime,
    [Name("(UTC+04:00) Yerevan")]
    CaucasusStandardTime,
    [Name("(UTC+04:30) Kabul")]
    AfghanistanStandardTime,
    [Name("(UTC+05:00) Ashgabat, Tashkent")]
    WestAsiaStandardTime,
    [Name("(UTC+05:00) Ekaterinburg")]
    EkaterinburgStandardTime,
    [Name("(UTC+05:00) Islamabad, Karachi")]
    PakistanStandardTime,
    [Name("(UTC+05:30) Chennai, Kolkata, Mumbai, New Delhi")]
    IndiaStandardTime,
    [Name("(UTC+05:30) Sri Jayawardenepura")]
    SriLankaStandardTime,
    [Name("(UTC+05:45) Kathmandu")]
    NepalStandardTime,
    [Name("(UTC+06:00) Astana")]
    CentralAsiaStandardTime,
    [Name("(UTC+06:00) Dhaka")]
    BangladeshStandardTime,
    [Name("(UTC+06:00) Novosibirsk")]
    NCentralAsiaStandardTime,
    [Name("(UTC+06:30) Yangon (Rangoon)")]
    MyanmarStandardTime,
    [Name("(UTC+07:00) Bangkok, Hanoi, Jakarta")]
    SEAsiaStandardTime,
    [Name("(UTC+07:00) Barnaul, Gorno-Altaysk")]
    AltaiStandardTime,
    [Name("(UTC+07:00) Hovd")]
    WMongoliaStandardTime,
    [Name("(UTC+07:00) Krasnoyarsk")]
    NorthAsiaStandardTime,
    [Name("(UTC+07:00) Tomsk")]
    TomskStandardTime,
    [Name("(UTC+08:00) Beijing, Chongqing, Hong Kong, Urumqi")]
    ChinaStandardTime,
    [Name("(UTC+08:00) Irkutsk")]
    NorthAsiaEastStandardTime,
    [Name("(UTC+08:00) Kuala Lumpur, Singapore")]
    SingaporeStandardTime,
    [Name("(UTC+08:00) Perth")]
    WAustraliaStandardTime,
    [Name("(UTC+08:00) Taipei")]
    TaipeiStandardTime,
    [Name("(UTC+08:00) Ulaanbaatar")]
    UlaanbaatarStandardTime,
    [Name("(UTC+08:30) Pyongyang")]
    NorthKoreaStandardTime,
    [Name("(UTC+08:45) Eucla")]
    AusCentralWStandardTime,
    [Name("(UTC+09:00) Chita")]
    TransbaikalStandardTime,
    [Name("(UTC+09:00) Osaka, Sapporo, Tokyo")]
    TokyoStandardTime,
    [Name("(UTC+09:00) Seoul")]
    KoreaStandardTime,
    [Name("(UTC+09:00) Yakutsk")]
    YakutskStandardTime,
    [Name("(UTC+09:30) Adelaide")]
    CenAustraliaStandardTime,
    [Name("(UTC+09:30) Darwin")]
    AUSCentralStandardTime,
    [Name("(UTC+10:00) Brisbane")]
    EAustraliaStandardTime,
    [Name("(UTC+10:00) Canberra, Melbourne, Sydney")]
    AUSEasternStandardTime,
    [Name("(UTC+10:00) Guam, Port Moresby")]
    WestPacificStandardTime,
    [Name("(UTC+10:00) Hobart")]
    TasmaniaStandardTime,
    [Name("(UTC+10:00) Vladivostok")]
    VladivostokStandardTime,
    [Name("(UTC+10:30) Lord Howe Island")]
    LordHoweStandardTime,
    [Name("(UTC+11:00) Bougainville Island")]
    BougainvilleStandardTime,
    [Name("(UTC+11:00) Chokurdakh")]
    RussiaTimeZone10,
    [Name("(UTC+11:00) Magadan")]
    MagadanStandardTime,
    [Name("(UTC+11:00) Norfolk Island")]
    NorfolkStandardTime,
    [Name("(UTC+11:00) Sakhalin")]
    SakhalinStandardTime,
    [Name("(UTC+11:00) Solomon Is., New Caledonia")]
    CentralPacificStandardTime,
    [Name("(UTC+12:00) Anadyr, Petropavlovsk-Kamchatsky")]
    RussiaTimeZone11,
    [Name("(UTC+12:00) Auckland, Wellington")]
    NewZealandStandardTime,
    [Name("(UTC+12:00) Coordinated Universal Time+12")]
    UTC12,
    [Name("(UTC+12:00) Fiji")]
    FijiStandardTime,
    [Name("(UTC+12:00) Petropavlovsk-Kamchatsky - Old")]
    KamchatkaStandardTime,
    [Name("(UTC+12:45) Chatham Islands")]
    ChathamIslandsStandardTime,
    [Name("(UTC+13:00) Nuku'alofa")]
    TongaStandardTime,
    [Name("(UTC+13:00) Samoa")]
    SamoaStandardTime,
    [Name("(UTC+14:00) Kiritimati Island")]
    LineIslandsStandardTime,
}