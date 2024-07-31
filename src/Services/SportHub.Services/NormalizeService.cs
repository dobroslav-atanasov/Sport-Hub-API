namespace SportHub.Services;

using System;
using System.Text.RegularExpressions;

using SportHub.Data.Models.Converters.OlympicGames;
using SportHub.Data.Models.Converters.OlympicGames.Olympedia.Base;
using SportHub.Data.Models.Enumerations.OlympicGames;
using SportHub.Services.Interfaces;

public class NormalizeService : INormalizeService
{
    public string CleanEventName(string text)
    {
        var name = string.Empty;
        if (text.StartsWith("Open"))
        {
            name = text.Replace("Open", string.Empty).Trim();
        }
        else
        {
            name = text.Replace("Men", string.Empty).Replace("Women", string.Empty).Replace("Mixed", string.Empty).Trim();
        }

        return name;
    }

    public string MapCityNameAndYearToNOCCode(string cityName, int year)
    {
        var text = $"{cityName} {year}";
        var code = string.Empty;
        switch (text)
        {
            case "Athens 1896": code = "GRE"; break;
            case "Paris 1900": code = "FRA"; break;
            case "St. Louis 1904": code = "USA"; break;
            case "London 1908": code = "GBR"; break;
            case "Stockholm 1912": code = "SWE"; break;
            case "Berlin 1916": code = "GER"; break;
            case "Antwerp 1920": code = "BEL"; break;
            case "Paris 1924": code = "FRA"; break;
            case "Amsterdam 1928": code = "NED"; break;
            case "Los Angeles 1932": code = "USA"; break;
            case "Berlin 1936": code = "GER"; break;
            case "Helsinki 1940": code = "FIN"; break;
            case "London 1944": code = "GBR"; break;
            case "London 1948": code = "GBR"; break;
            case "Helsinki 1952": code = "FIN"; break;
            case "Melbourne 1956": code = "AUS"; break;
            case "Rome 1960": code = "ITA"; break;
            case "Tokyo 1964": code = "JPN"; break;
            case "Mexico City 1968": code = "MEX"; break;
            case "Munich 1972": code = "FRG"; break;
            case "Montreal 1976": code = "CAN"; break;
            case "Moscow 1980": code = "URS"; break;
            case "Los Angeles 1984": code = "USA"; break;
            case "Seoul 1988": code = "KOR"; break;
            case "Barcelona 1992": code = "ESP"; break;
            case "Atlanta 1996": code = "USA"; break;
            case "Sydney 2000": code = "AUS"; break;
            case "Athens 2004": code = "GRE"; break;
            case "Beijing 2008": code = "CHN"; break;
            case "London 2012": code = "GBR"; break;
            case "Rio de Janeiro 2016": code = "BRA"; break;
            case "Tokyo 2020": code = "JPN"; break;
            case "Paris 2024": code = "FRA"; break;
            case "Los Angeles 2028": code = "USA"; break;
            case "Brisbane 2032": code = "AUS"; break;
            case "Chamonix 1924": code = "FRA"; break;
            case "St. Moritz 1928": code = "SUI"; break;
            case "Lake Placid 1932": code = "USA"; break;
            case "Garmisch-Partenkirchen 1936": code = "GER"; break;
            case "Garmisch-Partenkirchen 1940": code = "GER"; break;
            case "Cortina d'Ampezzo 1944": code = "ITA"; break;
            case "St. Moritz 1948": code = "SUI"; break;
            case "Oslo 1952": code = "NOR"; break;
            case "Cortina d'Ampezzo 1956": code = "ITA"; break;
            case "Squaw Valley 1960": code = "USA"; break;
            case "Innsbruck 1964": code = "AUT"; break;
            case "Grenoble 1968": code = "FRA"; break;
            case "Sapporo 1972": code = "JPN"; break;
            case "Innsbruck 1976": code = "AUT"; break;
            case "Lake Placid 1980": code = "USA"; break;
            case "Sarajevo 1984": code = "YUG"; break;
            case "Calgary 1988": code = "CAN"; break;
            case "Albertville 1992": code = "FRA"; break;
            case "Lillehammer 1994": code = "NOR"; break;
            case "Nagano 1998": code = "JPN"; break;
            case "Salt Lake City 2002": code = "USA"; break;
            case "Turin 2006": code = "ITA"; break;
            case "Vancouver 2010": code = "CAN"; break;
            case "Sochi 2014": code = "RUS"; break;
            case "PyeongChang 2018": code = "KOR"; break;
            case "Beijing 2022": code = "CHN"; break;
            case "Milano-Cortina d'Ampezzo 2026": code = "ITA"; break;
            case "Stockholm 1956": code = "SWE"; break;
        }

        return code;
    }

    public string MapOlympicGamesCountriesAndWorldCountries(string code)
    {
        return code switch
        {
            "AFG" => "AFG",
            "ALB" => "ALB",
            "ALG" => "DZA",
            "ASA" => "ASM",
            "AND" => "AND",
            "ANG" => "AGO",
            "ANT" => "ATG",
            "ARG" => "ARG",
            "ARM" => "ARM",
            "ARU" => "ABW",
            "AUS" => "AUS",
            "AUT" => "AUT",
            "AZE" => "AZE",
            "BAH" => "BHS",
            "BRN" => "BHR",
            "BAN" => "BGD",
            "BAR" => "BRB",
            "BLR" => "BLR",
            "BEL" => "BEL",
            "BIZ" => "BLZ",
            "BEN" => "BEN",
            "BER" => "BMU",
            "BHU" => "BTN",
            "BOL" => "BOL",
            "BIH" => "BIH",
            "BOT" => "BWA",
            "BRA" => "BRA",
            "IVB" => "VGB",
            "BRU" => "BRN",
            "BUL" => "BGR",
            "BUR" => "BFA",
            "BDI" => "BDI",
            "CAM" => "KHM",
            "CMR" => "CMR",
            "CAN" => "CAN",
            "CPV" => "CPV",
            "CAY" => "CYM",
            "CAF" => "CAF",
            "CHA" => "TCD",
            "CHI" => "CHL",
            "COL" => "COL",
            "COM" => "COM",
            "CGO" => "COG",
            "COK" => "COK",
            "CRC" => "CRI",
            "CIV" => "CIV",
            "CRO" => "HRV",
            "CUB" => "CUB",
            "CYP" => "CYP",
            "CZE" => "CZE",
            "PRK" => "PRK",
            "COD" => "COD",
            "DEN" => "DNK",
            "DJI" => "DJI",
            "DMA" => "DMA",
            "DOM" => "DOM",
            "ECU" => "ECU",
            "EGY" => "EGY",
            "ESA" => "SLV",
            "GEQ" => "GNQ",
            "ERI" => "ERI",
            "EST" => "EST",
            "SWZ" => "SWZ",
            "ETH" => "ETH",
            "FSM" => "FSM",
            "FIJ" => "FJI",
            "FIN" => "FIN",
            "FRA" => "FRA",
            "GAB" => "GAB",
            "GEO" => "GEO",
            "GER" => "DEU",
            "GHA" => "GHA",
            "GBR" => "GBR",
            "GRE" => "GRC",
            "GRN" => "GRD",
            "GUM" => "GUM",
            "GUA" => "GTM",
            "GUI" => "GIN",
            "GBS" => "GNB",
            "GUY" => "GUY",
            "HAI" => "HTI",
            "HON" => "HND",
            "HKG" => "HKG",
            "HUN" => "HUN",
            "ISL" => "ISL",
            "IND" => "IND",
            "INA" => "IDN",
            "IRQ" => "IRQ",
            "IRL" => "IRL",
            "IRI" => "IRN",
            "ISR" => "ISR",
            "ITA" => "ITA",
            "JAM" => "JAM",
            "JPN" => "JPN",
            "JOR" => "JOR",
            "KAZ" => "KAZ",
            "KEN" => "KEN",
            "KSA" => "SAU",
            "KIR" => "KIR",
            "KOS" => "UNK",
            "KUW" => "KWT",
            "KGZ" => "KGZ",
            "LAO" => "LAO",
            "LAT" => "LVA",
            "LBN" => "LBN",
            "LES" => "LSO",
            "LBR" => "LBR",
            "LBA" => "LBY",
            "LIE" => "LIE",
            "LTU" => "LTU",
            "LUX" => "LUX",
            "MAD" => "MDG",
            "MAW" => "MWI",
            "MAS" => "MYS",
            "MDV" => "MDV",
            "MLI" => "MLI",
            "MLT" => "MLT",
            "MHL" => "MHL",
            "MTN" => "MRT",
            "MRI" => "MUS",
            "MEX" => "MEX",
            "MON" => "MCO",
            "MGL" => "MNG",
            "MNE" => "MNE",
            "MAR" => "MAR",
            "MOZ" => "MOZ",
            "MYA" => "MMR",
            "NAM" => "NAM",
            "NRU" => "NRU",
            "NEP" => "NPL",
            "NED" => "NLD",
            "NZL" => "NZL",
            "NCA" => "NIC",
            "NIG" => "NER",
            "NGR" => "NGA",
            "MKD" => "MKD",
            "NOR" => "NOR",
            "OMA" => "OMN",
            "PAK" => "PAK",
            "PLW" => "PLW",
            "PLE" => "PSE",
            "PAN" => "PAN",
            "PNG" => "PNG",
            "PAR" => "PRY",
            "CHN" => "CHN",
            "PER" => "PER",
            "PHI" => "PHL",
            "POL" => "POL",
            "POR" => "PRT",
            "PUR" => "PRI",
            "QAT" => "QAT",
            "KOR" => "KOR",
            "MDA" => "MDA",
            "ROU" => "ROU",
            "RUS" => "RUS",
            "RWA" => "RWA",
            "SKN" => "KNA",
            "LCA" => "LCA",
            "VIN" => "VCT",
            "SAM" => "WSM",
            "SMR" => "SMR",
            "STP" => "STP",
            "SEN" => "SEN",
            "SRB" => "SRB",
            "YUG" => "SRB",
            "SEY" => "SYC",
            "SLE" => "SLE",
            "SGP" => "SGP",
            "SVK" => "SVK",
            "SLO" => "SVN",
            "SOL" => "SLB",
            "SOM" => "SOM",
            "RSA" => "ZAF",
            "SSD" => "SSD",
            "ESP" => "ESP",
            "SRI" => "LKA",
            "SUD" => "SDN",
            "SUR" => "SUR",
            "SWE" => "SWE",
            "SUI" => "CHE",
            "SYR" => "SYR",
            "TJK" => "TJK",
            "THA" => "THA",
            "GAM" => "GMB",
            "TLS" => "TLS",
            "TOG" => "TGO",
            "TGA" => "TON",
            "TTO" => "TTO",
            "TUN" => "TUN",
            "TUR" => "TUR",
            "TKM" => "TKM",
            "TUV" => "TUV",
            "UGA" => "UGA",
            "UKR" => "UKR",
            "UAE" => "ARE",
            "TAN" => "TZA",
            "USA" => "USA",
            "ISV" => "VIR",
            "URU" => "URY",
            "UZB" => "UZB",
            "VAN" => "VUT",
            "VEN" => "VEN",
            "VIE" => "VNM",
            "YEM" => "YEM",
            "ZAM" => "ZMB",
            "ZIM" => "ZWE",
            _ => null
        };
    }

    public string NormalizeEventName(OlympediaDocumentModel model)
    {
        var name = model.EventInfo.OriginalName;
        name = Regex.Replace(name, @"(\d+)\s+(\d+)", me =>
        {
            return $"{me.Groups[1].Value.Trim()}{me.Groups[2].Value.Trim()}";
        });

        name = Regex.Replace(name, @"(\d+),(\d+)", me =>
        {
            return $"{me.Groups[1].Value.Trim()}{me.Groups[2].Value.Trim()}";
        });

        name = name.Replace(" x ", "x")
            .Replace("82½", "82.5")
            .Replace("67½", "67.5")
            .Replace("333⅓", "333 1/3")
            .Replace(" × ", "x")
            .Replace("¼", "1/4")
            .Replace("⅓", "1/3")
            .Replace("½", "1/2")
            .Replace("²", string.Empty)
            .Replace("kilometer", "kilometers")
            .Replace("metres", "meters")
            .Replace("kilometres", "kilometers")
            .Replace("≤", "-")
            .Replace(">", "+");

        name = name.Replace(" / ", "/")
            .Replace(" meters", "m")
            .Replace(" kilometers", "km")
            .Replace(" miles", "miles")
            .Replace(" mile", "mile")
            .Replace(" km", "km")
            .Replace("Pommelled Horse", "Pommel Horse")
            .Replace("Teams", "Team")
            .Replace("Horse Vault", "Vault")
            .Replace("Alpine Combined", "Combined")
            .Replace("Super Combined", "Combined")
            .Replace("Birds", "Bird")
            .Replace("Pole Archery", "Fixed")
            .Replace("Apparatus Work and Field Sports", string.Empty)
            .Replace("Individual All-Around, Apparatus Work", "Triathlon")
            .Replace("Individual All-Around, 4 Events", "Combined")
            .Replace("European System", string.Empty)
            .Replace("Four/Five", "Four")
            .Replace("Canadian Singles", "C-1")
            .Replace("Canadian Doubles", "C-2")
            .Replace("Kayak Singles", "K-1")
            .Replace("Kayak Doubles", "K-2")
            .Replace("Kayak Fours", "K-4")
            .Replace("Kayak Relay", "K-1")
            .Replace("Two-Man Teams With Cesta", "Team")
            .Replace("Eights", "Eight")
            .Replace("Coxed Fours", "Coxed Four")
            .Replace("Coxed Teams", "Coxed Pair")
            .Replace("Coxless Fours", "Coxless Four")
            .Replace("Coxless Teams", "Coxless Pair")
            .Replace("Covered Courts", "Indoor")
            .Replace("Target Archery", "Moving Bird");

        if (model.GameCache.Year == 1924 && model.DisciplineCache.Name == "Artistic Gymnastics" && name == "Side Horse, Men")
        {
            name = "Pommel Horse, Men";
        }

        return name;
    }

    public string NormalizeHostCityName(string hostCity)
    {
        return hostCity switch
        {
            "Athina" => "Athens",
            "Antwerpen" => "Antwerp",
            "Ciudad de México" => "Mexico City",
            "Moskva" => "Moscow",
            "Sankt Moritz" => "St. Moritz",
            "Roma" => "Rome",
            "München" => "Munich",
            "Montréal" => "Montreal",
            "Torino" => "Turin",
            _ => hostCity
        };
    }

    public string ReplaceNonEnglishLetters(string name)
    {
        name = name.Replace("-", "-")
            .Replace("‐", "-")
            .Replace("–", "-")
            .Replace(",", string.Empty)
            .Replace(".", string.Empty)
            .Replace("'", string.Empty)
            .Replace("’", string.Empty)
            .Replace("(", string.Empty)
            .Replace(")", string.Empty)
            .Replace("`", string.Empty)
            .Replace("а", "a")
            .Replace("А", "A")
            .Replace("і", "i")
            .Replace("о", "o")
            .Replace("á", "а")
            .Replace("Á", "А")
            .Replace("à", "а")
            .Replace("À", "А")
            .Replace("ă", "а")
            .Replace("ằ", "а")
            .Replace("â", "а")
            .Replace("Â", "А")
            .Replace("ấ", "а")
            .Replace("ầ", "а")
            .Replace("ẩ", "а")
            .Replace("å", "а")
            .Replace("Å", "А")
            .Replace("ä", "а")
            .Replace("Ä", "А")
            .Replace("ã", "а")
            .Replace("ą", "а")
            .Replace("ā", "а")
            .Replace("Ā", "А")
            .Replace("ả", "а")
            .Replace("ạ", "а")
            .Replace("ặ", "а")
            .Replace("ậ", "а")
            .Replace("æ", "ае")
            .Replace("Æ", "Ae")
            .Replace("ć", "c")
            .Replace("Ć", "C")
            .Replace("č", "c")
            .Replace("Č", "C")
            .Replace("ç", "c")
            .Replace("Ç", "C")
            .Replace("ď", "d")
            .Replace("Ď", "D")
            .Replace("đ", "d")
            .Replace("Đ", "D")
            .Replace("ð", "d")
            .Replace("Ð", "D")
            .Replace("é", "e")
            .Replace("É", "E")
            .Replace("è", "e")
            .Replace("È", "E")
            .Replace("ĕ", "e")
            .Replace("ê", "e")
            .Replace("Ê", "E")
            .Replace("ế", "e")
            .Replace("ề", "e")
            .Replace("ễ", "e")
            .Replace("ể", "e")
            .Replace("ě", "e")
            .Replace("ë", "e")
            .Replace("ė", "e")
            .Replace("ę", "e")
            .Replace("ē", "e")
            .Replace("Ē", "E")
            .Replace("ệ", "e")
            .Replace("ə", "e")
            .Replace("Ə", "E")
            .Replace("Ǵ", "G")
            .Replace("ğ", "g")
            .Replace("ģ", "g")
            .Replace("Ģ", "G")
            .Replace("í", "i")
            .Replace("Í", "I")
            .Replace("ì", "i")
            .Replace("î", "i")
            .Replace("ï", "i")
            .Replace("İ", "I")
            .Replace("ī", "i")
            .Replace("ị", "i")
            .Replace("ı", "i")
            .Replace("ķ", "k")
            .Replace("Ķ", "K")
            .Replace("ľ", "l")
            .Replace("Ľ", "L")
            .Replace("ļ", "l")
            .Replace("ł", "l")
            .Replace("Ł", "L")
            .Replace("ń", "n")
            .Replace("ň", "n")
            .Replace("ñ", "n")
            .Replace("ņ", "n")
            .Replace("Ņ", "N")
            .Replace("ó", "o")
            .Replace("Ó", "O")
            .Replace("ò", "o")
            .Replace("ô", "o")
            .Replace("ố", "o")
            .Replace("ồ", "o")
            .Replace("ỗ", "o")
            .Replace("ö", "o")
            .Replace("Ö", "O")
            .Replace("ő", "o")
            .Replace("Ő", "O")
            .Replace("õ", "o")
            .Replace("Õ", "O")
            .Replace("ø", "o")
            .Replace("Ø", "O")
            .Replace("ơ", "o")
            .Replace("ớ", "o")
            .Replace("ờ", "o")
            .Replace("ọ", "o")
            .Replace("œ", "oe")
            .Replace("ř", "r")
            .Replace("Ř", "R")
            .Replace("ś", "s")
            .Replace("Ś", "S")
            .Replace("š", "s")
            .Replace("Š", "S")
            .Replace("ş", "s")
            .Replace("Ş", "S")
            .Replace("ș", "s")
            .Replace("Ș", "S")
            .Replace("ß", "ss")
            .Replace("ť", "t")
            .Replace("Ť", "T")
            .Replace("ţ", "t")
            .Replace("Ţ", "T")
            .Replace("ț", "t")
            .Replace("Ț", "T")
            .Replace("ú", "u")
            .Replace("Ú", "U")
            .Replace("ù", "u")
            .Replace("û", "u")
            .Replace("ů", "u")
            .Replace("ü", "u")
            .Replace("Ü", "U")
            .Replace("ű", "u")
            .Replace("ũ", "u")
            .Replace("ū", "u")
            .Replace("Ū", "U")
            .Replace("ủ", "u")
            .Replace("ư", "u")
            .Replace("ứ", "u")
            .Replace("ữ", "u")
            .Replace("ụ", "u")
            .Replace("ý", "y")
            .Replace("Ý", "Y")
            .Replace("ỳ", "y")
            .Replace("ÿ", "y")
            .Replace("ỹ", "y")
            .Replace("ỷ", "y")
            .Replace("ź", "z")
            .Replace("Ź", "Z")
            .Replace("ž", "z")
            .Replace("Ž", "Z")
            .Replace("ż", "z")
            .Replace("Ż", "Z")
            .Replace("þ", "th")
            .Replace("Þ", "Th")
            .Replace("ϊ", "i");

        return name;
    }

    public RoundDataModel MapRoundData(string name)
    {
        var round = new RoundDataModel
        {
            Name = name,
            Type = RoundEnum.None,
            SubType = RoundEnum.None
        };

        switch (name)
        {
            case "Barrage for 1/2": round.Type = RoundEnum.GoldMedalMatch; break;
            case "Classification 5-8": round.Type = RoundEnum.Classification; round.Info = "5-8"; break;
            case "Classification 9-12": round.Type = RoundEnum.Classification; round.Info = "9-12"; break;
            case "Classification Round": round.Type = RoundEnum.Classification; break;
            case "Classification Round 13-15": round.Type = RoundEnum.Classification; round.Info = "13-15"; break;
            case "Classification Round 13-16": round.Type = RoundEnum.Classification; round.Info = "13-16"; break;
            case "Classification Round 17-20": round.Type = RoundEnum.Classification; round.Info = "17-20"; break;
            case "Classification Round 17-23": round.Type = RoundEnum.Classification; round.Info = "17-23"; break;
            case "Classification Round 21-23": round.Type = RoundEnum.Classification; round.Info = "21-23"; break;
            case "Classification Round 2-3": round.Type = RoundEnum.SilverMedalMatch; break;
            case "Classification Round 3rd Place": round.Type = RoundEnum.BronzeMedalMatch; break;
            case "Classification Round 5-11": round.Type = RoundEnum.Classification; round.Info = "5-11"; break;
            case "Classification Round 5-8": round.Type = RoundEnum.Classification; round.Info = "5-8"; break;
            case "Classification Round 5-82": round.Type = RoundEnum.Classification; round.Info = "5-8"; break;
            case "Classification Round 7-10": round.Type = RoundEnum.Classification; round.Info = "7-10"; break;
            case "Classification Round 7-12": round.Type = RoundEnum.Classification; round.Info = "7-12"; break;
            case "Classification Round 9-11": round.Type = RoundEnum.Classification; round.Info = "9-11"; break;
            case "Classification Round 9-12": round.Type = RoundEnum.Classification; round.Info = "9-12"; break;
            case "Classification Round 9-123": round.Type = RoundEnum.Classification; round.Info = "9-12"; break;
            case "Classification Round 9-16": round.Type = RoundEnum.Classification; round.Info = "9-16"; break;
            case "Classification Round for 5/6": round.Type = RoundEnum.Classification; round.Info = "5-6"; break;
            case "Preliminary Round": round.Type = RoundEnum.PreliminaryRound; break;
            case "Quarter Finals": round.Type = RoundEnum.Quarterfinals; break;
            case "Quarter-Finals": round.Type = RoundEnum.Quarterfinals; break;
            case "Quarter-Finals, 64032": round.Type = RoundEnum.Quarterfinals; break;
            case "Quarter-Finals Repêchage": round.Type = RoundEnum.Quarterfinals; round.SubType = RoundEnum.Repechage; break;
            case "Quarter-Finals Repechage": round.Type = RoundEnum.Quarterfinals; round.SubType = RoundEnum.Repechage; break;
            case "Semi-Final": round.Type = RoundEnum.Semifinals; break;
            case "Semi-Final Round": round.Type = RoundEnum.Semifinals; break;
            case "Semi-Finals": round.Type = RoundEnum.Semifinals; break;
            case "Semi-Finals3": round.Type = RoundEnum.Semifinals; break;
            case "Semi-Finals Repechage": round.Type = RoundEnum.Semifinals; round.SubType = RoundEnum.Repechage; break;
            case "Semi-Finals Repêchage": round.Type = RoundEnum.Semifinals; round.SubType = RoundEnum.Repechage; break;
            case "Semi-Finals A/B": round.Type = RoundEnum.Semifinals; round.Info = "A-B"; break;
            case "Semi-Finals C/D": round.Type = RoundEnum.Semifinals; round.Info = "C-D"; break;
            case "Semi-Finals E/F": round.Type = RoundEnum.Semifinals; round.Info = "E-F"; break;
            case "1/8-Final Repechage": round.Type = RoundEnum.Eightfinals; round.SubType = RoundEnum.Repechage; break;
            case "1/8-Final Repechage Final": round.Type = RoundEnum.Eightfinals; round.SubType = RoundEnum.Repechage; break;
            case "1/8-Final Repêchage Final": round.Type = RoundEnum.Eightfinals; round.SubType = RoundEnum.Repechage; break;
            case "2nd-Place Final Round": round.Type = RoundEnum.Repechage; round.SubType = RoundEnum.SilverMedalMatch; break;
            case "2nd-Place Round One": round.Type = RoundEnum.Repechage; round.SubType = RoundEnum.SilverMedalMatch; break;
            case "2nd-Place Semi-Finals": round.Type = RoundEnum.Repechage; round.SubType = RoundEnum.SilverMedalMatch; break;
            case "2nd-Place Tournament": round.Type = RoundEnum.Repechage; round.SubType = RoundEnum.SilverMedalMatch; break;
            case "3rd-Place Final Round": round.Type = RoundEnum.Repechage; round.SubType = RoundEnum.BronzeMedalMatch; break;
            case "3rd-Place Quarter-Finals": round.Type = RoundEnum.Repechage; round.SubType = RoundEnum.BronzeMedalMatch; break;
            case "3rd-Place Round One": round.Type = RoundEnum.Repechage; round.SubType = RoundEnum.BronzeMedalMatch; break;
            case "3rd-Place Semi-Finals": round.Type = RoundEnum.Repechage; round.SubType = RoundEnum.BronzeMedalMatch; break;
            case "3rd-Place Tournament": round.Type = RoundEnum.Repechage; round.SubType = RoundEnum.BronzeMedalMatch; break;
            case "Group A": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Group; round.Number = 1; break;
            case "Group A - Final": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Group; round.Number = 1; break;
            case "Group A - Round Five": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Group; round.Number = 1; break;
            case "Group A - Round Four": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Group; round.Number = 1; break;
            case "Group A - Round One": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Group; round.Number = 1; break;
            case "Group A - Round Seven": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Group; round.Number = 1; break;
            case "Group A - Round Six": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Group; round.Number = 1; break;
            case "Group A - Round Three": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Group; round.Number = 1; break;
            case "Group A - Round Two": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Group; round.Number = 1; break;
            case "Group A1": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Group; round.Number = 1; break;
            case "Group B": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Group; round.Number = 2; break;
            case "Group B - Final": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Group; round.Number = 2; break;
            case "Group B - Round Five": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Group; round.Number = 2; break;
            case "Group B - Round Four": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Group; round.Number = 2; break;
            case "Group B - Round One": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Group; round.Number = 2; break;
            case "Group B - Round Seven": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Group; round.Number = 2; break;
            case "Group B - Round Six": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Group; round.Number = 2; break;
            case "Group B - Round Three": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Group; round.Number = 2; break;
            case "Group B - Round Two": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Group; round.Number = 2; break;
            case "Group B2": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Group; round.Number = 2; break;
            case "Group C": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Group; round.Number = 3; break;
            case "Group C3": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Group; round.Number = 3; break;
            case "Group D": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Group; round.Number = 4; break;
            case "Group E": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Group; round.Number = 5; break;
            case "Group F": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Group; round.Number = 6; break;
            case "Group G": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Group; round.Number = 7; break;
            case "Group H": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Group; round.Number = 8; break;
            case "Group I": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Group; round.Number = 9; break;
            case "Group J": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Group; round.Number = 10; break;
            case "Group K": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Group; round.Number = 11; break;
            case "Group L": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Group; round.Number = 12; break;
            case "Group M": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Group; round.Number = 13; break;
            case "Group N": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Group; round.Number = 14; break;
            case "Group O": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Group; round.Number = 15; break;
            case "Group One": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Group; round.Number = 1; break;
            case "Group P": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Group; round.Number = 16; break;
            case "Group Two": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Group; round.Number = 2; break;
            case "Heat #1": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Heat; round.Number = 1; break;
            case "Heat #1 Re-Race": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Heat; round.Number = 1; break;
            case "Heat #10": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Heat; round.Number = 10; break;
            case "Heat #11": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Heat; round.Number = 11; break;
            case "Heat #12": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Heat; round.Number = 12; break;
            case "Heat #13": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Heat; round.Number = 13; break;
            case "Heat #14": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Heat; round.Number = 14; break;
            case "Heat #15": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Heat; round.Number = 15; break;
            case "Heat #16": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Heat; round.Number = 16; break;
            case "Heat #17": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Heat; round.Number = 17; break;
            case "Heat #2": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Heat; round.Number = 2; break;
            case "Heat #3": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Heat; round.Number = 3; break;
            case "Heat #4": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Heat; round.Number = 4; break;
            case "Heat #5": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Heat; round.Number = 5; break;
            case "Heat #6": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Heat; round.Number = 6; break;
            case "Heat #7": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Heat; round.Number = 7; break;
            case "Heat #8": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Heat; round.Number = 8; break;
            case "Heat #9": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Heat; round.Number = 9; break;
            case "Heat 1/2": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Heat; round.Info = "1-2"; break;
            case "Heat 1-6": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Heat; round.Info = "1-6"; break;
            case "Heat 3/4": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Heat; round.Info = "3-4"; break;
            case "Heat 5/6": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Heat; round.Info = "5-6"; break;
            case "Heat 5-8": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Heat; round.Info = "5-8"; break;
            case "Heat 7/8": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Heat; round.Info = "7-8"; break;
            case "Heat 7-12": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Heat; round.Info = "7-12"; break;
            case "Heat 9-12": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Heat; round.Info = "9-12"; break;
            case "Heat Eight": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Heat; round.Number = 8; break;
            case "Heat Eighteen": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Heat; round.Number = 18; break;
            case "Heat Eleven": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Heat; round.Number = 11; break;
            case "Heat Fifteen": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Heat; round.Number = 15; break;
            case "Heat Five": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Heat; round.Number = 5; break;
            case "Heat Four": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Heat; round.Number = 4; break;
            case "Heat Fourteen": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Heat; round.Number = 14; break;
            case "Heat Nine": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Heat; round.Number = 9; break;
            case "Heat One": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Heat; round.Number = 1; break;
            case "Heat One Re-Run": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Heat; round.Number = 1; round.Info = "Playoff"; break;
            case "Heat Seven": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Heat; round.Number = 7; break;
            case "Heat Seventeen": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Heat; round.Number = 17; break;
            case "Heat Six": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Heat; round.Number = 6; break;
            case "Heat Six Re-Run": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Heat; round.Number = 6; round.Info = "Playoff"; break;
            case "Heat Sixteen": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Heat; round.Number = 16; break;
            case "Heat Ten": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Heat; round.Number = 10; break;
            case "Heat Thirteen": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Heat; round.Number = 13; break;
            case "Heat Three": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Heat; round.Number = 3; break;
            case "Heat Three Re-run": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Heat; round.Number = 3; round.Info = "Playoff"; break;
            case "Heat Twelve": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Heat; round.Number = 12; break;
            case "Heat Two": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Heat; round.Number = 2; break;
            case "Heat Two Re-run": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Heat; round.Number = 2; round.Info = "Playoff"; break;
            case "Pool 1": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 1; break;
            case "Pool 1, Barrage": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 1; round.Info = "Barrage"; break;
            case "Pool 1, Barrage 2-5": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 1; round.Info = "Barrage"; break;
            case "Pool 1, Barrage 3-4": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 1; round.Info = "Barrage"; break;
            case "Pool 1, Barrage 3-5": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 1; round.Info = "Barrage"; break;
            case "Pool 1, Barrage 3-6": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 1; round.Info = "Barrage"; break;
            case "Pool 1, Barrage 4-5": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 1; round.Info = "Barrage"; break;
            case "Pool 1, Barrage 4-6": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 1; round.Info = "Barrage"; break;
            case "Pool 1, Barrage 6-8": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 1; round.Info = "Barrage"; break;
            case "Pool 10": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 10; break;
            case "Pool 10, Barrage 2-4": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 10; round.Info = "Barrage"; break;
            case "Pool 10, Barrage 3-4": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 10; round.Info = "Barrage"; break;
            case "Pool 11": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 11; break;
            case "Pool 11, Barrage 2-4": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 11; round.Info = "Barrage"; break;
            case "Pool 11, Barrage 3-5": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 11; round.Info = "Barrage"; break;
            case "Pool 12": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 12; break;
            case "Pool 12, Barrage 2-4": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 12; round.Info = "Barrage"; break;
            case "Pool 12, Barrage 3-4": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 12; round.Info = "Barrage"; break;
            case "Pool 13": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 13; break;
            case "Pool 14": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 14; break;
            case "Pool 15": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 15; break;
            case "Pool 16": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 16; break;
            case "Pool 17": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 17; break;
            case "Pool 2": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 2; break;
            case "Pool 2, Barrage 2-4": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 2; round.Info = "Barrage"; break;
            case "Pool 2, Barrage 3-4": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 2; round.Info = "Barrage"; break;
            case "Pool 2, Barrage 3-5": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 2; round.Info = "Barrage"; break;
            case "Pool 2, Barrage 3-7": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 2; round.Info = "Barrage"; break;
            case "Pool 2, Barrage 4-5": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 2; round.Info = "Barrage"; break;
            case "Pool 2, Barrage 4-6": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 2; round.Info = "Barrage"; break;
            case "Pool 2, Barrage 5-6": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 2; round.Info = "Barrage"; break;
            case "Pool 2, Barrage 5-8": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 2; round.Info = "Barrage"; break;
            case "Pool 2, Barrage 6-12": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 2; round.Info = "Barrage"; break;
            case "Pool 3": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 3; break;
            case "Pool 3, Barrage 3-5": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 3; round.Info = "Barrage"; break;
            case "Pool 3, Barrage 4-5": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 3; round.Info = "Barrage"; break;
            case "Pool 3, Barrage 4-6": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 3; round.Info = "Barrage"; break;
            case "Pool 3, Barrage 5-6": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 3; round.Info = "Barrage"; break;
            case "Pool 3, Barrage 6-8": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 3; round.Info = "Barrage"; break;
            case "Pool 4": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 4; break;
            case "Pool 4, Barrage": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 4; round.Info = "Barrage"; break;
            case "Pool 4, Barrage 2-4": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 4; round.Info = "Barrage"; break;
            case "Pool 4, Barrage 2-5": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 4; round.Info = "Barrage"; break;
            case "Pool 4, Barrage 3-4": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 4; round.Info = "Barrage"; break;
            case "Pool 4, Barrage 3-5": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 4; round.Info = "Barrage"; break;
            case "Pool 4, Barrage 4-5": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 4; round.Info = "Barrage"; break;
            case "Pool 4, Barrage 4-6": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 4; round.Info = "Barrage"; break;
            case "Pool 4, Barrage 6-8": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 4; round.Info = "Barrage"; break;
            case "Pool 5": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 5; break;
            case "Pool 5, Barrage 2-4": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 5; round.Info = "Barrage"; break;
            case "Pool 5, Barrage 3-4": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 5; round.Info = "Barrage"; break;
            case "Pool 5, Barrage 3-6": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 5; round.Info = "Barrage"; break;
            case "Pool 5, Barrage 4-6": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 5; round.Info = "Barrage"; break;
            case "Pool 5, Barrage 5-7": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 5; round.Info = "Barrage"; break;
            case "Pool 6": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 6; break;
            case "Pool 6, Barrage": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 6; round.Info = "Barrage"; break;
            case "Pool 6, Barrage 3-4": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 6; round.Info = "Barrage"; break;
            case "Pool 6, Barrage 3-5": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 6; round.Info = "Barrage"; break;
            case "Pool 6, Barrage 4-5": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 6; round.Info = "Barrage"; break;
            case "Pool 6, Barrage 5-6": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 6; round.Info = "Barrage"; break;
            case "Pool 7": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 7; break;
            case "Pool 7, Barrage 2-4": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 7; round.Info = "Barrage"; break;
            case "Pool 7, Barrage 3-5": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 7; round.Info = "Barrage"; break;
            case "Pool 7, Barrage 4-6": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 7; round.Info = "Barrage"; break;
            case "Pool 8": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 8; break;
            case "Pool 8, Barrage 2-4": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 8; round.Info = "Barrage"; break;
            case "Pool 8, Barrage 3-4": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 8; round.Info = "Barrage"; break;
            case "Pool 8, Barrage 3-5": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 8; round.Info = "Barrage"; break;
            case "Pool 8, Barrage 4-5": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 8; round.Info = "Barrage"; break;
            case "Pool 9": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 9; break;
            case "Pool A": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 1; break;
            case "Pool B": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 2; break;
            case "Pool C": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 3; break;
            case "Pool D": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 4; break;
            case "Pool E": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 5; break;
            case "Pool F": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 6; break;
            case "Pool Five": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 5; break;
            case "Pool Four": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 4; break;
            case "Pool G": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 7; break;
            case "Pool H": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 8; break;
            case "Pool One": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 1; break;
            case "Pool Three": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 3; break;
            case "Pool Two": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Pool; round.Number = 2; break;
            case "Round Five": round.Type = RoundEnum.RoundFive; break;
            case "Round Four": round.Type = RoundEnum.RoundFour; break;
            case "Round Four5": round.Type = RoundEnum.RoundFour; break;
            case "Round One": round.Type = RoundEnum.RoundOne; break;
            case "Round One Pool Five": round.Type = RoundEnum.RoundOne; round.SubType = RoundEnum.Pool; round.Number = 5; break;
            case "Round One Pool Four": round.Type = RoundEnum.RoundOne; round.SubType = RoundEnum.Pool; round.Number = 4; break;
            case "Round One Pool One": round.Type = RoundEnum.RoundOne; round.SubType = RoundEnum.Pool; round.Number = 1; break;
            case "Round One Pool Six": round.Type = RoundEnum.RoundOne; round.SubType = RoundEnum.Pool; round.Number = 6; break;
            case "Round One Pool Three": round.Type = RoundEnum.RoundOne; round.SubType = RoundEnum.Pool; round.Number = 3; break;
            case "Round One Pool Two": round.Type = RoundEnum.RoundOne; round.SubType = RoundEnum.Pool; round.Number = 2; break;
            case "Round One Repechage":
            case "Round One Repêchage": round.Type = RoundEnum.RoundOne; round.SubType = RoundEnum.Repechage; break;
            case "Round One Repechage Final": round.Type = RoundEnum.RoundOne; round.SubType = RoundEnum.Repechage; break;
            case "Round One Rerace": round.Type = RoundEnum.RoundOne; round.SubType = RoundEnum.Playoff; break;
            case "Round One, Heat Ten": round.Type = RoundEnum.RoundOne; round.SubType = RoundEnum.Heat; round.Number = 10; break;
            case "Round One1": round.Type = RoundEnum.RoundOne; break;
            case "Round One9": round.Type = RoundEnum.RoundOne; break;
            case "Round Seven": round.Type = RoundEnum.RoundSeven; break;
            case "Round Six": round.Type = RoundEnum.RoundSix; break;
            case "Round Three": round.Type = RoundEnum.RoundThree; break;
            case "Round Three Repechage": round.Type = RoundEnum.RoundThree; round.SubType = RoundEnum.Repechage; break;
            case "Round Two": round.Type = RoundEnum.RoundTwo; break;
            case "Round Two Repechage":
            case "Round Two Repêchage": round.Type = RoundEnum.RoundTwo; round.SubType = RoundEnum.Repechage; break;
            case "Round Two Repechage Final": round.Type = RoundEnum.RoundTwo; round.SubType = RoundEnum.Repechage; break;
            case "Round-Robin": round.Type = RoundEnum.RoundRobin; break;
            case "Round Robin": round.Type = RoundEnum.RoundRobin; break;
            case "Eighth-Finals": round.Type = RoundEnum.Eightfinals; break;
            case "Elimination Round": round.Type = RoundEnum.EliminationRound; break;
            case "Elimination Rounds, Round Five Repechage": round.Type = RoundEnum.RoundFive; round.SubType = RoundEnum.Repechage; break;
            case "Elimination Rounds, Round Five Repêchage": round.Type = RoundEnum.RoundFive; round.SubType = RoundEnum.Repechage; break;
            case "Elimination Rounds, Round Four": round.Type = RoundEnum.RoundFour; break;
            case "Elimination Rounds, Round Four Repechage": round.Type = RoundEnum.RoundFour; round.SubType = RoundEnum.Repechage; break;
            case "Elimination Rounds, Round Four Repêchage": round.Type = RoundEnum.RoundFour; round.SubType = RoundEnum.Repechage; break;
            case "Elimination Rounds, Round One": round.Type = RoundEnum.RoundOne; break;
            case "Elimination Rounds, Round One Repechage": round.Type = RoundEnum.RoundOne; round.SubType = RoundEnum.Repechage; break;
            case "Elimination Rounds, Round One Repêchage": round.Type = RoundEnum.RoundOne; round.SubType = RoundEnum.Repechage; break;
            case "Elimination Rounds, Round Three": round.Type = RoundEnum.RoundThree; break;
            case "Elimination Rounds, Round Three Repechage": round.Type = RoundEnum.RoundThree; round.SubType = RoundEnum.Repechage; break;
            case "Elimination Rounds, Round Three Repêchage": round.Type = RoundEnum.RoundThree; round.SubType = RoundEnum.Repechage; break;
            case "Elimination Rounds, Round Two": round.Type = RoundEnum.RoundTwo; break;
            case "Elimination Rounds, Round Two Repechage": round.Type = RoundEnum.RoundTwo; round.SubType = RoundEnum.Repechage; break;
            case "Elimination Rounds, Round Two Repêchage": round.Type = RoundEnum.RoundTwo; round.SubType = RoundEnum.Repechage; break;
            case "Compulsory Dance": round.Type = RoundEnum.CompulsoryDance; break;
            case "Compulsory Dance 1": round.Type = RoundEnum.CompulsoryDance; break;
            case "Compulsory Dance 2": round.Type = RoundEnum.CompulsoryDance; break;
            case "Compulsory Dances": round.Type = RoundEnum.CompulsoryDance; break;
            case "Compulsory Dances Summary": round.Type = RoundEnum.CompulsoryDance; break;
            case "Compulsory Figures": round.Type = RoundEnum.CompulsoryFigures; break;
            case "Figures": round.Type = RoundEnum.CompulsoryFigures; break;
            case "Consolation Final": round.Type = RoundEnum.ConsolationRound; round.SubType = RoundEnum.Final; break;
            case "Consolation Round": round.Type = RoundEnum.ConsolationRound; break;
            case "Consolation Round - Final": round.Type = RoundEnum.ConsolationRound; round.SubType = RoundEnum.Final; break;
            case "Consolation Round - Round One": round.Type = RoundEnum.ConsolationRound; round.SubType = RoundEnum.RoundOne; break;
            case "Consolation Round - Semi-Finals": round.Type = RoundEnum.ConsolationRound; round.SubType = RoundEnum.Semifinals; break;
            case "Consolation Round: Final": round.Type = RoundEnum.ConsolationRound; round.SubType = RoundEnum.Final; break;
            case "Consolation Round: Quarter-Finals": round.Type = RoundEnum.ConsolationRound; round.SubType = RoundEnum.Quarterfinals; break;
            case "Consolation Round: Semi-Finals": round.Type = RoundEnum.ConsolationRound; round.SubType = RoundEnum.Semifinals; break;
            case "Consolation Tournament": round.Type = RoundEnum.ConsolationRound; break;
            case "Qualification": round.Type = RoundEnum.Qualification; break;
            case "Qualification Round": round.Type = RoundEnum.Qualification; break;
            case "Qualifying": round.Type = RoundEnum.Qualification; break;
            case "Qualifying Round": round.Type = RoundEnum.Qualification; break;
            case "Qualifying Round 1": round.Type = RoundEnum.RoundOne; break;
            case "Qualifying Round 2": round.Type = RoundEnum.RoundTwo; break;
            case "Qualifying Round One": round.Type = RoundEnum.RoundOne; break;
            case "Qualifying Round Two": round.Type = RoundEnum.RoundTwo; break;
            case "Qualifying Round, Group A": round.Type = RoundEnum.Qualification; round.SubType = RoundEnum.Group; round.Number = 1; break;
            case "Qualifying Round, Group A Re-Jump": round.Type = RoundEnum.Qualification; round.SubType = RoundEnum.Group; round.Number = 1; round.Info = "Playoff"; break;
            case "Qualifying Round, Group A1": round.Type = RoundEnum.Qualification; round.SubType = RoundEnum.Group; round.Number = 1; break;
            case "Qualifying Round, Group B": round.Type = RoundEnum.Qualification; round.SubType = RoundEnum.Group; round.Number = 2; break;
            case "Qualifying Round, Group B1": round.Type = RoundEnum.Qualification; round.SubType = RoundEnum.Group; round.Number = 2; break;
            case "Qualifying Round, Group C": round.Type = RoundEnum.Qualification; round.SubType = RoundEnum.Group; round.Number = 3; break;
            case "Qualifying Round, Group C3": round.Type = RoundEnum.Qualification; round.SubType = RoundEnum.Group; round.Number = 3; break;
            case "Qualifying Round, Group D": round.Type = RoundEnum.Qualification; round.SubType = RoundEnum.Group; round.Number = 4; break;
            case "Qualifying Round, Group D4": round.Type = RoundEnum.Qualification; round.SubType = RoundEnum.Group; round.Number = 4; break;
            case "Qualifying Round, Group E": round.Type = RoundEnum.Qualification; round.SubType = RoundEnum.Group; round.Number = 5; break;
            case "Qualifying Round, Group F": round.Type = RoundEnum.Qualification; round.SubType = RoundEnum.Group; round.Number = 6; break;
            case "Qualifying Round, Group One": round.Type = RoundEnum.Qualification; round.SubType = RoundEnum.Group; round.Number = 1; break;
            case "Qualifying Round, Group Two": round.Type = RoundEnum.Qualification; round.SubType = RoundEnum.Group; round.Number = 2; break;
            case "Classification Final 1": round.Type = RoundEnum.Classification; round.SubType = RoundEnum.Final; break;
            case "Classification Final 2": round.Type = RoundEnum.Classification; round.SubType = RoundEnum.Final; break;
            case "Classification Round Five": round.Type = RoundEnum.Classification; round.SubType = RoundEnum.RoundFive; break;
            case "Classification Round Four": round.Type = RoundEnum.Classification; round.SubType = RoundEnum.RoundFour; break;
            case "Classification Round One": round.Type = RoundEnum.Classification; round.SubType = RoundEnum.RoundOne; break;
            case "Classification Round Six": round.Type = RoundEnum.Classification; round.SubType = RoundEnum.RoundSix; break;
            case "Classification Round Three": round.Type = RoundEnum.Classification; round.SubType = RoundEnum.RoundThree; break;
            case "Classification Round Two": round.Type = RoundEnum.Classification; round.SubType = RoundEnum.RoundTwo; break;
            case "Race Eight": round.Type = RoundEnum.RaceEight; break;
            case "Race Five": round.Type = RoundEnum.RaceFive; break;
            case "Race Four": round.Type = RoundEnum.RaceFour; break;
            case "Race Nine": round.Type = RoundEnum.RaceNine; break;
            case "Race One": round.Type = RoundEnum.RaceOne; break;
            case "Race Seven": round.Type = RoundEnum.RaceSeven; break;
            case "Race Six": round.Type = RoundEnum.RaceSix; break;
            case "Race Ten": round.Type = RoundEnum.RaceTen; break;
            case "Race Three": round.Type = RoundEnum.RaceThree; break;
            case "Race Two": round.Type = RoundEnum.RaceTwo; break;
            case "Ranking Round": round.Type = RoundEnum.RankingRound; break;
            case "Lucky Loser Round": round.Type = RoundEnum.LuckyLoserRound; break;
            case "Jump-Off": round.Type = RoundEnum.Playoff; break;
            case "Jump-Off for 1-2": round.Type = RoundEnum.Playoff; round.Info = "1-2"; break;
            case "Jump-off for 2-4": round.Type = RoundEnum.Playoff; round.Info = "2-4"; break;
            case "Jump-Off for 3-4": round.Type = RoundEnum.Playoff; round.Info = "3-4"; break;
            case "Jump-off for 3-5": round.Type = RoundEnum.Playoff; round.Info = "3-5"; break;
            case "Jump-Off for 3-9": round.Type = RoundEnum.Playoff; round.Info = "3-9"; break;
            case "Jump-off for 6-7": round.Type = RoundEnum.Playoff; round.Info = "6-7"; break;
            case "Seeding Round": round.Type = RoundEnum.RankingRound; break;
            case "Shoot-Off": round.Type = RoundEnum.Playoff; break;
            case "Shoot-Off 1": round.Type = RoundEnum.Playoff; break;
            case "Shoot-Off 2": round.Type = RoundEnum.Playoff; break;
            case "Shoot-Off for 1st Place": round.Type = RoundEnum.Playoff; round.SubType = RoundEnum.GoldMedalMatch; break;
            case "Shoot-Off for 2nd Place": round.Type = RoundEnum.Playoff; round.SubType = RoundEnum.SilverMedalMatch; break;
            case "Shoot-Off for 3rd Place": round.Type = RoundEnum.Playoff; round.SubType = RoundEnum.BronzeMedalMatch; break;
            case "Short Dance": round.Type = RoundEnum.ShortProgram; break;
            case "Short Program": round.Type = RoundEnum.ShortProgram; break;
            case "Swim-Off": round.Type = RoundEnum.Playoff; break;
            case "Swim-Off for 16th Place": round.Type = RoundEnum.Playoff; round.Info = "16-17"; break;
            case "Swim-Off for 16th Place - Race 1": round.Type = RoundEnum.Playoff; round.Info = "16-17"; break;
            case "Swim-Off for 16th Place - Race 2": round.Type = RoundEnum.Playoff; round.Info = "16-17"; break;
            case "Swim-Off for 8th Place": round.Type = RoundEnum.Playoff; round.Info = "8-9"; break;
            case "Swim-Off for Places 7-8": round.Type = RoundEnum.Playoff; round.Info = "7-8"; break;
            case "Third-Place Tournament": round.Type = RoundEnum.Repechage; round.SubType = RoundEnum.BronzeMedalMatch; break;
            case "Tie-Breaker": round.Type = RoundEnum.Playoff; break;
            case "Second Place Tournament - Final": round.Type = RoundEnum.Repechage; round.SubType = RoundEnum.SilverMedalMatch; break;
            case "Second Place Tournament - Round One": round.Type = RoundEnum.Repechage; round.SubType = RoundEnum.RoundOne; break;
            case "Second Place Tournament - Round Two": round.Type = RoundEnum.Repechage; round.SubType = RoundEnum.RoundTwo; break;
            case "Second Place Tournament - Semi-Finals": round.Type = RoundEnum.Repechage; round.SubType = RoundEnum.Semifinals; break;
            case "Second-Place Tournament": round.Type = RoundEnum.Repechage; round.SubType = RoundEnum.SilverMedalMatch; break;
            case "Second-to-Fifth Place Tournament": round.Type = RoundEnum.Repechage; round.SubType = RoundEnum.SilverMedalMatch; break;
            case "Match 1/2": round.Type = RoundEnum.GoldMedalMatch; break;
            case "Match 1-6": round.Type = RoundEnum.GoldMedalMatch; break;
            case "Match 3/4": round.Type = RoundEnum.BronzeMedalMatch; break;
            case "Match 5-7": round.Type = RoundEnum.Classification; round.Info = "5-7"; break;
            case "Match 5-8": round.Type = RoundEnum.Classification; round.Info = "5-8"; break;
            case "Match 7-10": round.Type = RoundEnum.Classification; round.Info = "7-10"; break;
            case "Match 9-12": round.Type = RoundEnum.Classification; round.Info = "9-12"; break;
            case "Grand Prix": round.Type = RoundEnum.GrandPrix; break;
            case "Grand Prix Freestyle": round.Type = RoundEnum.GrandPrix; break;
            case "Grand Prix Special": round.Type = RoundEnum.GrandPrix; break;
            case "Free Dance": round.Type = RoundEnum.FreeSkating; break;
            case "Free Skating": round.Type = RoundEnum.FreeSkating; break;
            case "Play-Off for Bronze Medal": round.Type = RoundEnum.BronzeMedalMatch; break;
            case "Play-Off for Silver Medal": round.Type = RoundEnum.SilverMedalMatch; break;
            case "Play-offs": round.Type = RoundEnum.Playoff; break;
            case "Repechage": round.Type = RoundEnum.Repechage; break;
            case "Repêchage": round.Type = RoundEnum.Repechage; break;
            case "Repêchage Final": round.Type = RoundEnum.Repechage; round.SubType = RoundEnum.Final; break;
            case "Repechage Final": round.Type = RoundEnum.Repechage; round.SubType = RoundEnum.Final; break;
            case "Repechage Heats": round.Type = RoundEnum.Repechage; break;
            case "Repechage Round One": round.Type = RoundEnum.Repechage; round.SubType = RoundEnum.RoundOne; break;
            case "Repechage Round Two": round.Type = RoundEnum.Repechage; round.SubType = RoundEnum.RoundTwo; break;
            case "Rhythm Dance": round.Type = RoundEnum.RhythmDance; break;
            case "A Final": round.Type = RoundEnum.Final; round.Info = "A"; break;
            case "B Final": round.Type = RoundEnum.Final; round.Info = "B"; break;
            case "Final": round.Type = RoundEnum.Final; break;
            case "Final A": round.Type = RoundEnum.Final; round.Info = "A"; break;
            case "Final B": round.Type = RoundEnum.Final; round.Info = "B"; break;
            case "Final C": round.Type = RoundEnum.Final; round.Info = "C"; break;
            case "Final D": round.Type = RoundEnum.Final; round.Info = "D"; break;
            case "Final E": round.Type = RoundEnum.Final; round.Info = "E"; break;
            case "Final F": round.Type = RoundEnum.Final; round.Info = "F"; break;
            case "Final Heat": round.Type = RoundEnum.Repechage; break;
            case "Final Heat One": round.Type = RoundEnum.Repechage; break;
            case "Final Heat Two": round.Type = RoundEnum.Repechage; break;
            case "Final Pool": round.Type = RoundEnum.FinalRound; break;
            case "Final Pool Barrage 2-3": round.Type = RoundEnum.SilverMedalMatch; break;
            case "Final Pool, Barrage #1 1-2": round.Type = RoundEnum.GoldMedalMatch; break;
            case "Final Pool, Barrage #2 1-2": round.Type = RoundEnum.GoldMedalMatch; break;
            case "Final Pool, Barrage 1-2": round.Type = RoundEnum.GoldMedalMatch; break;
            case "Final Pool, Barrage 1-3": round.Type = RoundEnum.FinalRound; break;
            case "Final Pool, Barrage 1-4": round.Type = RoundEnum.FinalRound; break;
            case "Final Pool, Barrage 2-3": round.Type = RoundEnum.FinalRound; break;
            case "Final Pool, Barrage 2-4": round.Type = RoundEnum.FinalRound; break;
            case "Final Pool, Barrage 2-5": round.Type = RoundEnum.FinalRound; break;
            case "Final Pool, Barrage 3-4": round.Type = RoundEnum.BronzeMedalMatch; break;
            case "Final Pool, Barrage 3-5": round.Type = RoundEnum.BronzeMedalMatch; break;
            case "Final Pool, Barrage 4-5": round.Type = RoundEnum.Classification; round.Info = "4-5"; break;
            case "Final Pool, Barrage 6-7": round.Type = RoundEnum.Classification; round.Info = "6-7"; break;
            case "Final Round": round.Type = RoundEnum.FinalRound; break;
            case "Final Round 1": round.Type = RoundEnum.FinalRound; break;
            case "Final Round 2": round.Type = RoundEnum.FinalRound; break;
            case "Final Round 3": round.Type = RoundEnum.FinalRound; break;
            case "Final Round One": round.Type = RoundEnum.FinalRound; break;
            case "Final Round Three": round.Type = RoundEnum.FinalRound; break;
            case "Final Round Two": round.Type = RoundEnum.FinalRound; break;
            case "Final Round2": round.Type = RoundEnum.FinalRound; break;
            case "Final, Swim-Off": round.Type = RoundEnum.Final; round.SubType = RoundEnum.Playoff; break;
            case "Final1": round.Type = RoundEnum.Final; break;
            case "First Final": round.Type = RoundEnum.Final; break;
            case "Fleet Races": round.Type = RoundEnum.FleetRaces; break;
            case "Medal Pool": round.Type = RoundEnum.RoundTwo; break;
            case "Original Final": round.Type = RoundEnum.Final; break;
            case "Original Round One": round.Type = RoundEnum.RoundOne; break;
            case "Original Set Pattern Dance": round.Type = RoundEnum.OriginalSetPatternDance; break;
            case "Re-run Final": round.Type = RoundEnum.Playoff; break;
            case "Re-run of Heat Two": round.Type = RoundEnum.PreliminaryRound; round.SubType = RoundEnum.Heat; round.Number = 2; round.Info = "Playoff"; break;
            case "Free Routine": round.Type = RoundEnum.FreeRoutine; break;
            case "Technical Routine": round.Type = RoundEnum.TechnicalRoutine; break;
        }

        return round;
    }

    public RoundEnum MapAdditionalRound(string name)
    {
        var round = RoundEnum.None;

        switch (name)
        {
            case "Downhill":
            case "Downhill1":
                round = RoundEnum.Downhill;
                break;
            case "Run #1":
            case "Run #11":
                round = RoundEnum.Run1;
                break;
            case "Run #2":
            case "Run #21":
                round = RoundEnum.Run2;
                break;
            case "Slalom":
            case "Slalom1":
                round = RoundEnum.Slalom;
                break;
        }

        return round;
    }

    public string MapCityToCountry(string city)
    {
        var country = string.Empty;
        switch (city)
        {
            case "Albertville": country = "France"; break;
            case "Amsterdam": country = "Netherlands"; break;
            case "Antwerp": country = "Belgium"; break;
            case "Athens": country = "Greece"; break;
            case "Atlanta": country = "United States"; break;
            case "Barcelona": country = "Spain"; break;
            case "Beijing": country = "China"; break;
            case "Berlin": country = "Germany"; break;
            case "Brisbane": country = "Australia"; break;
            case "Calgary": country = "Canada"; break;
            case "Chamonix": country = "France"; break;
            case "Cortina d'Ampezzo": country = "Italy"; break;
            case "Garmisch-Partenkirchen": country = "Germany"; break;
            case "Grenoble": country = "France"; break;
            case "Helsinki": country = "Finland"; break;
            case "Innsbruck": country = "Austria"; break;
            case "Lake Placid": country = "United States"; break;
            case "Lillehammer": country = "Norway"; break;
            case "London": country = "United Kingdom"; break;
            case "Los Angeles": country = "United States"; break;
            case "Melbourne": country = "Australia"; break;
            case "Mexico City": country = "Mexico"; break;
            case "Milano-Cortina d'Ampezzo": country = "Italy"; break;
            case "Montreal": country = "Canada"; break;
            case "Moscow": country = "Russia"; break;
            case "Munich": country = "Germany"; break;
            case "Nagano": country = "Japan"; break;
            case "Oslo": country = "Norway"; break;
            case "Paris": country = "France"; break;
            case "PyeongChang": country = "South Korea"; break;
            case "Rio de Janeiro": country = "Brazil"; break;
            case "Rome": country = "Italy"; break;
            case "Salt Lake City": country = "United States"; break;
            case "Sapporo": country = "Japan"; break;
            case "Sarajevo": country = "Yugoslavia"; break;
            case "Seoul": country = "South Korea"; break;
            case "Sochi": country = "Russia"; break;
            case "Squaw Valley": country = "United States"; break;
            case "St. Louis": country = "United States"; break;
            case "St. Moritz": country = "Switzerland"; break;
            case "Stockholm": country = "Sweden"; break;
            case "Sydney": country = "Australia"; break;
            case "Tokyo": country = "Japan"; break;
            case "Turin": country = "Italy"; break;
            case "Vancouver": country = "Canada"; break;
        }

        return country;
    }

    public Tuple<string, string> MapDisciplineToSport(string discipline)
    {
        Tuple<string, string> sport = null;
        sport = new Tuple<string, string>("", "");
        switch (discipline)
        {
            case "Archery": sport = new Tuple<string, string>("Archery", "ARC"); break;
            case "Athletics": sport = new Tuple<string, string>("Athletics", "ATH"); break;
            case "Badminton": sport = new Tuple<string, string>("Badminton", "BDM"); break;
            case "3x3 Basketball":
            case "Basketball":
                sport = new Tuple<string, string>("Basketball", "BSK");
                break;
            case "Breaking": sport = new Tuple<string, string>("Breaking", "BKG"); break;
            case "Cycling BMX Freestyle":
            case "Cycling Road":
            case "Cycling Track":
            case "Cycling BMX Racing":
            case "Cycling Mountain Bike":
                sport = new Tuple<string, string>("Cycling", "CYC");
                break;
            case "Boxing": sport = new Tuple<string, string>("Boxing", "BOX"); break;
            case "Sport Climbing": sport = new Tuple<string, string>("Sport Climbing", "CLB"); break;
            case "Canoe Slalom":
            case "Canoe Sprint":
                sport = new Tuple<string, string>("Canoe", "CAS");
                break;
            case "Diving":
            case "Marathon Swimming":
            case "Artistic Swimming":
            case "Water Polo":
            case "Swimming":
                sport = new Tuple<string, string>("Aquatics", "AQU");
                break;
            case "Equestrian": sport = new Tuple<string, string>("Equestrian", "EQU"); break;
            case "Football": sport = new Tuple<string, string>("Football", "FBL"); break;
            case "Fencing": sport = new Tuple<string, string>("Fencing", "FEN"); break;
            case "Artistic Gymnastics":
            case "Rhythmic Gymnastics":
            case "Trampoline Gymnastics":
                sport = new Tuple<string, string>("Gymnastics", "GYM");
                break;
            case "Golf": sport = new Tuple<string, string>("Golf", "GLF"); break;
            case "Handball": sport = new Tuple<string, string>("Handball", "HBL"); break;
            case "Hockey": sport = new Tuple<string, string>("Hockey", "HOC"); break;
            case "Judo": sport = new Tuple<string, string>("Judo", "JUD"); break;
            case "Modern Pentathlon": sport = new Tuple<string, string>("Modern Pentathlon", "MPN"); break;
            case "Rowing": sport = new Tuple<string, string>("Rowing", "ROW"); break;
            case "Rugby":
            case "Rugby Sevens":
                sport = new Tuple<string, string>("Rugby", "RUG");
                break;
            case "Sailing": sport = new Tuple<string, string>("Sailing", "SAL"); break;
            case "Shooting": sport = new Tuple<string, string>("Shooting", "SHO"); break;
            case "Skateboarding": sport = new Tuple<string, string>("Skateboarding", "SKB"); break;
            case "Surfing": sport = new Tuple<string, string>("Surfing", "SRF"); break;
            case "Tennis": sport = new Tuple<string, string>("Tennis", "TEN"); break;
            case "Taekwondo": sport = new Tuple<string, string>("Taekwondo", "TKW"); break;
            case "Triathlon": sport = new Tuple<string, string>("Triathlon", "TRI"); break;
            case "Table Tennis": sport = new Tuple<string, string>("Table Tennis", "TTE"); break;
            case "Beach Volleyball":
            case "Volleyball":
                sport = new Tuple<string, string>("Volleyball", "VOL");
                break;
            case "Weightlifting": sport = new Tuple<string, string>("Weightlifting", "WLF"); break;
            case "Wrestling": sport = new Tuple<string, string>("Wrestling", "WRE"); break;
            case "Alpine Skiing":
            case "Cross Country Skiing":
            case "Nordic Combined":
            case "Ski Jumping":
            case "Snowboarding":
            case "Freestyle Skiing":
                sport = new Tuple<string, string>("Skiing", "SKI");
                break;
            case "Baseball":
            case "Softball":
                sport = new Tuple<string, string>("Baseball and Softball", "BAS");
                break;
            case "Basque pelota": sport = new Tuple<string, string>("Basque pelota", "PEL"); break;
            case "Biathlon": sport = new Tuple<string, string>("Biathlon", "BTH"); break;
            case "Bobsleigh":
            case "Skeleton":
                sport = new Tuple<string, string>("Bobsleigh", "BOB");
                break;
            case "Cricket": sport = new Tuple<string, string>("Cricket", "CKT"); break;
            case "Croquet": sport = new Tuple<string, string>("Croquet", "CQT"); break;
            case "Curling": sport = new Tuple<string, string>("Curling", "CUR"); break;
            case "Figure Skating":
            case "Short Track Speed Skating":
            case "Speed Skating":
                sport = new Tuple<string, string>("Skating", "SKA");
                break;
            case "Ice Hockey": sport = new Tuple<string, string>("Ice Hockey", "IHO"); break;
            case "Jeu De Paume": sport = new Tuple<string, string>("Jeu De Paume", "JDP"); break;
            case "Karate": sport = new Tuple<string, string>("Karate", "KTE"); break;
            case "Lacrosse": sport = new Tuple<string, string>("Lacrosse", "LAX"); break;
            case "Luge": sport = new Tuple<string, string>("Luge", "LUG"); break;
            case "Military Ski Patrol": sport = new Tuple<string, string>("Military Ski Patrol", "MPT"); break;
            case "Motorboating": sport = new Tuple<string, string>("Motorboating", "PBT"); break;
            case "Polo": sport = new Tuple<string, string>("Polo", "POL"); break;
            case "Racquets": sport = new Tuple<string, string>("Racquets", "RQT"); break;
            case "Roque": sport = new Tuple<string, string>("Roque", "RQE"); break;
            case "Tug-Of-War": sport = new Tuple<string, string>("Tug-Of-War", "TOW"); break;
        }

        return sport;
    }

    public string NormalizeDisciplineName(string name)
    {
        switch (name)
        {
            case "Canoe Marathon":
                name = "Canoe Sprint"; break;
            case "Equestrian Dressage":
            case "Equestrian Driving":
            case "Equestrian Eventing":
            case "Equestrian Jumping":
            case "Equestrian Vaulting":
                name = "Equestrian"; break;
            case "Trampolining":
                name = "Trampoline Gymnastics"; break;
            case "Tug-Of-War":
                name = "Tug-Of-War"; break;
        }

        return name;
    }
}