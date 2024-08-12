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

        name = name.Replace("82½", "82.5")
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

        //if (model.Game.Year == 1924 && model.Discipline.Name == "Artistic Gymnastics" && name == "Side Horse, Men")
        //{
        //    name = "Pommel Horse, Men";
        //}

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

    public string GetShortEventName(string name)
    {
        var shortName = string.Empty;

        switch (name)
        {
            case "+100kg": shortName = "O100KG"; break;
            case "+105kg": shortName = "O105KG"; break;
            case "+108kg": shortName = "O108KG"; break;
            case "+109kg": shortName = "O109KG"; break;
            case "+110kg": shortName = "O110KG"; break;
            case "+67kg": shortName = "O67KG"; break;
            case "+71.67kg": shortName = "O71.67KG"; break;
            case "+72kg": shortName = "O72KG"; break;
            case "+75kg": shortName = "O75KG"; break;
            case "+78kg": shortName = "O78KG"; break;
            case "+79.38kg": shortName = "O79.38KG"; break;
            case "+80kg": shortName = "O80KG"; break;
            case "+81kg": shortName = "O81KG"; break;
            case "+82.5kg": shortName = "O82.5KG"; break;
            case "+87kg": shortName = "O87KG"; break;
            case "+90kg": shortName = "O90KG"; break;
            case "+91kg": shortName = "O91KG"; break;
            case "+93kg": shortName = "O93KG"; break;
            case "+95kg": shortName = "O95KG"; break;
            case "0-1/2 Ton Race One": shortName = "0TONRO"; break;
            case "0-1/2 Ton Race Two": shortName = "0TONRT"; break;
            case "1/2-1 Ton Race One": shortName = "1TONRO"; break;
            case "1/2-1 Ton Race Two": shortName = "1TONRT"; break;
            case "1/2mile": shortName = "1.2MILE"; break;
            case "1/3mile": shortName = "1.3MILE"; break;
            case "1/4mile": shortName = "1.4MILE"; break;
            case "10/10km Pursuit": shortName = "1010KMPUR"; break;
            case "10/15km Pursuit": shortName = "1015KMPUR"; break;
            case "100 Yards Backstroke": shortName = "100YBA"; break;
            case "100 Yards Freestyle": shortName = "100YFR"; break;
            case "10000m": shortName = "10000M"; break;
            case "1000m": shortName = "1000M"; break;
            case "1000m Freestyle": shortName = "1000MFR"; break;
            case "1000m Time Trial": shortName = "1000MTT"; break;
            case "100kg": shortName = "100KG"; break;
            case "100km": shortName = "100KM"; break;
            case "100m": shortName = "100M"; break;
            case "100m Backstroke": shortName = "100MBA"; break;
            case "100m Breaststroke": shortName = "100MBR"; break;
            case "100m Butterfly": shortName = "100MBF"; break;
            case "100m Freestyle": shortName = "100MFR"; break;
            case "100m Freestyle For Sailors": shortName = "100MFRS"; break;
            case "100m Hurdles": shortName = "100MHURD"; break;
            case "10-20 Ton": shortName = "1020TON"; break;
            case "105kg": shortName = "105KG"; break;
            case "108kg": shortName = "108KG"; break;
            case "109kg": shortName = "109KG"; break;
            case "10km": shortName = "10KM"; break;
            case "10km Open Water": shortName = "10KMOW"; break;
            case "10km Pursuit": shortName = "10KMPUR"; break;
            case "10km Race Walk": shortName = "10KMWALK"; break;
            case "10km Sprint": shortName = "10KMSPRINT"; break;
            case "10m": shortName = "10M"; break;
            case "10m 1907 Rating": shortName = "10M1907"; break;
            case "10m 1919 Rating": shortName = "10M1919"; break;
            case "10miles Race Walk": shortName = "10MILEWALK"; break;
            case "110kg": shortName = "110KG"; break;
            case "110m Hurdles": shortName = "110MHURD"; break;
            case "1-2 Ton Race One": shortName = "12TONONE"; break;
            case "1-2 Ton Race Two": shortName = "12TONTWO"; break;
            case "12.5km Mass Start": shortName = "12.5MS"; break;
            case "12.5km Pursuit": shortName = "12.5PUR"; break;
            case "1200m Freestyle": shortName = "1200MFR"; break;
            case "12-Foot Dinghy": shortName = "12DINGHY"; break;
            case "12-Hours Race": shortName = "12HOURS"; break;
            case "12m": shortName = "12M"; break;
            case "12m 1907 Rating": shortName = "12M1907"; break;
            case "12m 1919 Rating": shortName = "12M1919"; break;
            case "1500m": shortName = "1500M"; break;
            case "1500m Freestyle": shortName = "1500MFR"; break;
            case "15km": shortName = "15KM"; break;
            case "15km Mass Start": shortName = "15KMMS"; break;
            case "15km Skiathlon": shortName = "15KMSKTL"; break;
            case "1600m Medley Relay": shortName = "1600MRELAY"; break;
            case "18-Foot Dinghy": shortName = "18DINGHY"; break;
            case "18km": shortName = "18KM"; break;
            case "1mile": shortName = "1MILE"; break;
            case "1mile Freestyle": shortName = "1MILEFR"; break;
            case "20+ Ton": shortName = "20TON"; break;
            case "2000m Relay": shortName = "2000MRELAY"; break;
            case "200m": shortName = "200M"; break;
            case "200m Backstroke": shortName = "200MBA"; break;
            case "200m Breaststroke": shortName = "200MBR"; break;
            case "200m Butterfly": shortName = "200MBF"; break;
            case "200m Freestyle": shortName = "200MFR"; break;
            case "200m Hurdles": shortName = "200MHURD"; break;
            case "200m Obstacle Course": shortName = "200MOC"; break;
            case "20km": shortName = "20KM"; break;
            case "20km Race Walk": shortName = "20KMWALK"; break;
            case "220 Yards Freestyle": shortName = "220YFR"; break;
            case "2-3 Ton Race One": shortName = "23TONONE"; break;
            case "2-3 Ton Race Two": shortName = "23TONTWO"; break;
            case "2500m Steeplechase": shortName = "2500MST"; break;
            case "2590m Steeplechase": shortName = "2590MST"; break;
            case "25km": shortName = "25KM"; break;
            case "25miles": shortName = "25MILE"; break;
            case "2miles": shortName = "2MILE"; break;
            case "2x6km And 2x7.5km Relay": shortName = "RELAY"; break;
            case "3000m": shortName = "3000M"; break;
            case "3000m Race Walk": shortName = "3000MWALK"; break;
            case "3000m Relay": shortName = "3000MRELAY"; break;
            case "3000m Steeplechase": shortName = "3000MST"; break;
            case "300m Freestyle": shortName = "300MFR"; break;
            case "30km": shortName = "30KM"; break;
            case "30km Skiathlon": shortName = "30KMSKTL"; break;
            case "30m": shortName = "30M"; break;
            case "3-10 Ton Race One": shortName = "310TONONE"; break;
            case "3-10 Ton Race Two": shortName = "310TONTWO"; break;
            case "3200m Steeplechase": shortName = "3200MST"; break;
            case "333 1/3m Time Trial": shortName = "333MTT"; break;
            case "3500m Race Walk": shortName = "3500MWALK"; break;
            case "3x3 Basketball": shortName = "TEAM"; break;
            case "3x5km Relay": shortName = "3X5KM"; break;
            case "3x7.5km Relay": shortName = "3X7.5KM"; break;
            case "4 X 100m Freestyle Relay": shortName = "4X100MFR"; break;
            case "4 X 100m Medley Relay": shortName = "4X100MMD"; break;
            case "4 X 100m Relay": shortName = "4X100M"; break;
            case "4 X 200m Freestyle Relay": shortName = "4X200MFR"; break;
            case "4 X 400m Relay": shortName = "4X400M"; break;
            case "4 X 50 Yards Freestyle Relay": shortName = "4X50YFR"; break;
            case "4000m Freestyle": shortName = "4000MFR"; break;
            case "4000m Steeplechase": shortName = "4000MST"; break;
            case "400m": shortName = "400M"; break;
            case "400m Breaststroke": shortName = "400MBR"; break;
            case "400m Freestyle": shortName = "400MFR"; break;
            case "400m Hurdles": shortName = "400MHURD"; break;
            case "40m": shortName = "40M"; break;
            case "440 Yards Breaststroke": shortName = "440YBR"; break;
            case "440 Yards Freestyle": shortName = "440YFR"; break;
            case "47.63kg": shortName = "47.63KG"; break;
            case "48kg": shortName = "48KG"; break;
            case "49kg": shortName = "49KG"; break;
            case "4x100m Relay": shortName = "4X100M"; break;
            case "4x10km Relay": shortName = "4X10KM"; break;
            case "4x400m Relay": shortName = "4X400M"; break;
            case "4x5km Relay": shortName = "4X5KM"; break;
            case "4x6km Relay": shortName = "4X6KM"; break;
            case "4x7.5km Relay": shortName = "4X7.5KM"; break;
            case "5.5m": shortName = "5.5M"; break;
            case "5/10km Pursuit": shortName = "510KMPUR"; break;
            case "5/5km Pursuit": shortName = "55KMPUR"; break;
            case "50 Yards Freestyle": shortName = "50YFR"; break;
            case "50.80kg": shortName = "50.80KG"; break;
            case "5000m": shortName = "5000M"; break;
            case "5000m Relay": shortName = "5000MRELAY"; break;
            case "500m": shortName = "500M"; break;
            case "500m Freestyle": shortName = "500MFR"; break;
            case "500m Time Trial": shortName = "500MTT"; break;
            case "50km": shortName = "50KM"; break;
            case "50km Race Walk": shortName = "50KMWALK"; break;
            case "50m Freestyle": shortName = "50MFR"; break;
            case "51kg": shortName = "51KG"; break;
            case "52.16kg": shortName = "52.16KG"; break;
            case "52.62kg": shortName = "52.62KG"; break;
            case "52kg": shortName = "52KG"; break;
            case "53.52kg": shortName = "53.52KG"; break;
            case "53kg": shortName = "53KG"; break;
            case "54kg": shortName = "54KG"; break;
            case "55kg": shortName = "55KG"; break;
            case "56.70kg": shortName = "56.70KG"; break;
            case "56kg": shortName = "56KG"; break;
            case "56-pound Weight Throw": shortName = "56PWT"; break;
            case "57.15kg": shortName = "57KG"; break;
            case "57kg": shortName = "58KG"; break;
            case "58kg": shortName = "59KG"; break;
            case "59kg": shortName = "5KM"; break;
            case "5km": shortName = "5MILES"; break;
            case "5miles": shortName = "5MILE"; break;
            case "6.5m 1919 Rating": shortName = "6.5M1919"; break;
            case "60kg": shortName = "60KG"; break;
            case "60m": shortName = "60M"; break;
            case "61.23kg": shortName = "61.23KG"; break;
            case "61kg": shortName = "61KG"; break;
            case "62kg": shortName = "62KG"; break;
            case "63.50kg": shortName = "63.50KG"; break;
            case "63.5kg": shortName = "63.5KG"; break;
            case "63kg": shortName = "63KG"; break;
            case "64kg": shortName = "64KG"; break;
            case "65.77kg": shortName = "65.77KG"; break;
            case "65kg": shortName = "65KG"; break;
            case "66.68kg": shortName = "66.68KG"; break;
            case "66kg": shortName = "66KG"; break;
            case "67.5kg": shortName = "67.5KG"; break;
            case "67kg": shortName = "67KG"; break;
            case "68kg": shortName = "68KG"; break;
            case "69kg": shortName = "69KG"; break;
            case "6m": shortName = "6M"; break;
            case "6m 1907 Rating": shortName = "6M1907"; break;
            case "6m 1919 Rating": shortName = "6M1919"; break;
            case "7.5km Sprint": shortName = "7.5KMSPRT"; break;
            case "70kg": shortName = "70KG"; break;
            case "71.67kg": shortName = "71.67KG"; break;
            case "71kg": shortName = "71KG"; break;
            case "72.57kg": shortName = "72.57KG"; break;
            case "72kg": shortName = "72KG"; break;
            case "73kg": shortName = "73KG"; break;
            case "75kg": shortName = "75KG"; break;
            case "76kg": shortName = "76KG"; break;
            case "77kg": shortName = "77KG"; break;
            case "78kg": shortName = "78KG"; break;
            case "79.38kg": shortName = "79.38KG"; break;
            case "7m": shortName = "7M"; break;
            case "7m 1907 Rating": shortName = "7M1907"; break;
            case "800m": shortName = "800M"; break;
            case "800m Freestyle": shortName = "800MFR"; break;
            case "80kg": shortName = "80KG"; break;
            case "80m Hurdles": shortName = "80MHURD"; break;
            case "81kg": shortName = "81KG"; break;
            case "82.5kg": shortName = "82.5KG"; break;
            case "83kg": shortName = "83KG"; break;
            case "85kg": shortName = "85KG"; break;
            case "86kg": shortName = "86KG"; break;
            case "87kg": shortName = "87KG"; break;
            case "880 Yards Freestyle": shortName = "880YFR"; break;
            case "8m": shortName = "8M"; break;
            case "8m 1907 Rating": shortName = "8M1907"; break;
            case "8m 1919 Rating": shortName = "8M1919"; break;
            case "90kg": shortName = "90KG"; break;
            case "91kg": shortName = "91KG"; break;
            case "93kg": shortName = "93KG"; break;
            case "94kg": shortName = "94KG"; break;
            case "95kg": shortName = "95KG"; break;
            case "96kg": shortName = "96KG"; break;
            case "99kg": shortName = "99KG"; break;
            case "A-Class": shortName = "ACLASS"; break;
            case "Aerials": shortName = "AERIALS"; break;
            case "Air Pistol 10m": shortName = "AP"; break;
            case "Air Rifle 10m": shortName = "AR"; break;
            case "All-Around Championship": shortName = "AA"; break;
            case "All-Around Dumbbell Contest": shortName = "AADC"; break;
            case "Allround": shortName = "AA"; break;
            case "Au Chapelet 33m": shortName = "AC33M"; break;
            case "Au Chapelet 50m": shortName = "AC50M"; break;
            case "Au Cordon Doré 33m": shortName = "ACD33M"; break;
            case "Au Cordon Doré 50m": shortName = "ACD50M"; break;
            case "Balance Beam": shortName = "BB"; break;
            case "Baseball": shortName = "TEAM"; break;
            case "Basketball": shortName = "TEAM"; break;
            case "B-Class": shortName = "BCLASS"; break;
            case "Beach Volleyball": shortName = "TEAM"; break;
            case "Big Air": shortName = "BIGAIR"; break;
            case "BMX": shortName = "BMX"; break;
            case "C-1 10000m": shortName = "C1-10000M"; break;
            case "C-1 1000m": shortName = "C1-1000M"; break;
            case "C-1 200m": shortName = "C1-200M"; break;
            case "C-1 500m": shortName = "C1-500M"; break;
            case "C-1 Slalom": shortName = "C1"; break;
            case "C-2 10000m": shortName = "C2-10000M"; break;
            case "C-2 1000m": shortName = "C2-1000M"; break;
            case "C-2 500m": shortName = "C2-500M"; break;
            case "C-2 Slalom": shortName = "C2"; break;
            case "C-Class": shortName = "CCLASS"; break;
            case "Championnat Du Monde": shortName = "CDM"; break;
            case "Club Swinging": shortName = "CW"; break;
            case "Combined": shortName = "COMBINED"; break;
            case "Continental Style": shortName = "CS"; break;
            case "Course De Primes": shortName = "CDP"; break;
            case "Coxed Four": shortName = "COX4"; break;
            case "Coxed Four 1": shortName = "COX4-1"; break;
            case "Coxed Four 2": shortName = "COX4-2"; break;
            case "Coxed Four Inriggers": shortName = "COX4I"; break;
            case "Coxed Four Outriggers": shortName = "COX4O"; break;
            case "Coxed Pairs": shortName = "COX2"; break;
            case "Coxed Quadruple Sculls": shortName = "COX4SCULL"; break;
            case "Coxless Four": shortName = "NOCOX4"; break;
            case "Coxless Pairs": shortName = "NOCOX2"; break;
            case "Cricket": shortName = "TEAM"; break;
            case "Cross": shortName = "CROSS"; break;
            case "Cross-Country": shortName = "CC"; break;
            case "Curling": shortName = "TEAM"; break;
            case "Decathlon": shortName = "DECATH"; break;
            case "Discus Throw": shortName = "DISCUS"; break;
            case "Discus Throw Both Hands": shortName = "DISCUSBH"; break;
            case "Discus Throw Greek Style": shortName = "DISCUSGS"; break;
            case "Double American Round": shortName = "DAR"; break;
            case "Double Columbia Round": shortName = "DCR"; break;
            case "Double National Round": shortName = "DNR"; break;
            case "Double Sculls": shortName = "SCULL2"; break;
            case "Double Trap": shortName = "DTRAP"; break;
            case "Double York Round": shortName = "DYR"; break;
            case "Doubles": shortName = "DOUBLES"; break;
            case "Doubles Indoor": shortName = "DOUBLESIN"; break;
            case "Downhill": shortName = "DOWNHILL"; break;
            case "Dressage Hacks And Hunter Combined": shortName = "DRESSHHC"; break;
            case "Driving Four-In-Hand Competition": shortName = "DRIVING"; break;
            case "Dueling Pistol 30m": shortName = "DPISTOL30M"; break;
            case "Duet": shortName = "DUET"; break;
            case "Eight": shortName = "COXED8"; break;
            case "Floor Exercise": shortName = "FX"; break;
            case "Folding K-1 10000m": shortName = "FK1-10000M"; break;
            case "Folding K-2 10000m": shortName = "FK2-10000M"; break;
            case "Football": shortName = "TEAM"; break;
            case "Four": shortName = "SCULL4"; break;
            case "Free Pistol 30m": shortName = "P30M"; break;
            case "Free Pistol 50 Yards": shortName = "P50Y"; break;
            case "Free Pistol 50m": shortName = "P50M"; break;
            case "Free Rifle 1000 Yards": shortName = "R1000Y"; break;
            case "Free Rifle Any Position 300m": shortName = "RAP300M"; break;
            case "Free Rifle Kneeling 300m": shortName = "RK300M"; break;
            case "Free Rifle Prone 300m": shortName = "RP300M"; break;
            case "Free Rifle Prone 600m": shortName = "RP600M"; break;
            case "Free Rifle Standing 300m": shortName = "RS300M"; break;
            case "Free Rifle Three Positions 300m": shortName = "R3P300M"; break;
            case "Freestyle +100kg": shortName = "FSO100KG"; break;
            case "Freestyle +71.67kg": shortName = "FSO71.67KG"; break;
            case "Freestyle +73kg": shortName = "FSO73KG"; break;
            case "Freestyle +82.5kg": shortName = "FSO82.5KG"; break;
            case "Freestyle +87kg": shortName = "FSO87KG"; break;
            case "Freestyle +97kg": shortName = "FSO97KG"; break;
            case "Freestyle 100kg": shortName = "FS100KG"; break;
            case "Freestyle 120kg": shortName = "FS120KG"; break;
            case "Freestyle 125kg": shortName = "FS125KG"; break;
            case "Freestyle 130kg": shortName = "FS130KG"; break;
            case "Freestyle 47.63kg": shortName = "FS47.63KG"; break;
            case "Freestyle 48kg": shortName = "FS48KG"; break;
            case "Freestyle 50kg": shortName = "FS50KG"; break;
            case "Freestyle 52.16kg": shortName = "FS52.16KG"; break;
            case "Freestyle 52kg": shortName = "FS52KG"; break;
            case "Freestyle 53kg": shortName = "FS53KG"; break;
            case "Freestyle 54kg": shortName = "FS54KG"; break;
            case "Freestyle 55kg": shortName = "FS55KG"; break;
            case "Freestyle 56.70kg": shortName = "FS56.70KG"; break;
            case "Freestyle 56kg": shortName = "FS56KG"; break;
            case "Freestyle 57kg": shortName = "FS57KG"; break;
            case "Freestyle 58kg": shortName = "FS58KG"; break;
            case "Freestyle 60.33kg": shortName = "FS60.33KG"; break;
            case "Freestyle 60kg": shortName = "FS60KG"; break;
            case "Freestyle 61.23kg": shortName = "FS61.23KG"; break;
            case "Freestyle 61kg": shortName = "FS61KG"; break;
            case "Freestyle 62kg": shortName = "FS62KG"; break;
            case "Freestyle 63kg": shortName = "FS63KG"; break;
            case "Freestyle 65.77kg": shortName = "FS65.77KG"; break;
            case "Freestyle 65kg": shortName = "FS65KG"; break;
            case "Freestyle 66.56kg": shortName = "FS66.56KG"; break;
            case "Freestyle 66kg": shortName = "FS66KG"; break;
            case "Freestyle 67.5kg": shortName = "FS67.5KG"; break;
            case "Freestyle 67kg": shortName = "FS67KG"; break;
            case "Freestyle 68kg": shortName = "FS68KG"; break;
            case "Freestyle 69kg": shortName = "FS69KG"; break;
            case "Freestyle 70kg": shortName = "FS70KG"; break;
            case "Freestyle 71.67kg": shortName = "FS71.67KG"; break;
            case "Freestyle 72kg": shortName = "FS72KG"; break;
            case "Freestyle 73kg": shortName = "FS73KG"; break;
            case "Freestyle 74kg": shortName = "FS74KG"; break;
            case "Freestyle 75kg": shortName = "FS75KG"; break;
            case "Freestyle 76kg": shortName = "FS76KG"; break;
            case "Freestyle 78kg": shortName = "FS78KG"; break;
            case "Freestyle 79kg": shortName = "FS79KG"; break;
            case "Freestyle 82.5kg": shortName = "FS82.5KG"; break;
            case "Freestyle 82kg": shortName = "FS82KG"; break;
            case "Freestyle 84kg": shortName = "FS84KG"; break;
            case "Freestyle 85kg": shortName = "FS85KG"; break;
            case "Freestyle 86kg": shortName = "FS86KG"; break;
            case "Freestyle 87kg": shortName = "FS87KG"; break;
            case "Freestyle 90kg": shortName = "FS90KG"; break;
            case "Freestyle 96kg": shortName = "FS96KG"; break;
            case "Freestyle 97kg": shortName = "FS97KG"; break;
            case "Giant Slalom": shortName = "GS"; break;
            case "Greco-Roman +100kg": shortName = "GRO100KG"; break;
            case "Greco-Roman +82.5kg": shortName = "GRO82.5KG"; break;
            case "Greco-Roman +87kg": shortName = "GRO87KG"; break;
            case "Greco-Roman +93kg": shortName = "GRO93KG"; break;
            case "Greco-Roman +97kg": shortName = "GRO97KG"; break;
            case "Greco-Roman 100kg": shortName = "GR100KG"; break;
            case "Greco-Roman 120kg": shortName = "GR120KG"; break;
            case "Greco-Roman 130kg": shortName = "GR130KG"; break;
            case "Greco-Roman 48kg": shortName = "GR48KG"; break;
            case "Greco-Roman 52kg": shortName = "GR52KG"; break;
            case "Greco-Roman 54kg": shortName = "GR54KG"; break;
            case "Greco-Roman 55kg": shortName = "GR55KG"; break;
            case "Greco-Roman 56kg": shortName = "GR56KG"; break;
            case "Greco-Roman 57kg": shortName = "GR57KG"; break;
            case "Greco-Roman 58kg": shortName = "GR58KG"; break;
            case "Greco-Roman 59kg": shortName = "GR59KG"; break;
            case "Greco-Roman 60kg": shortName = "GR60KG"; break;
            case "Greco-Roman 61kg": shortName = "GR61KG"; break;
            case "Greco-Roman 62kg": shortName = "GR62KG"; break;
            case "Greco-Roman 63kg": shortName = "GR63KG"; break;
            case "Greco-Roman 66.6kg": shortName = "GR66.6KG"; break;
            case "Greco-Roman 66kg": shortName = "GR66KG"; break;
            case "Greco-Roman 67.5kg": shortName = "GR67.5KG"; break;
            case "Greco-Roman 67kg": shortName = "GR67KG"; break;
            case "Greco-Roman 68kg": shortName = "GR68KG"; break;
            case "Greco-Roman 69kg": shortName = "GR69KG"; break;
            case "Greco-Roman 70kg": shortName = "GR70KG"; break;
            case "Greco-Roman 72kg": shortName = "GR72KG"; break;
            case "Greco-Roman 73kg": shortName = "GR73KG"; break;
            case "Greco-Roman 74kg": shortName = "GR74KG"; break;
            case "Greco-Roman 75kg": shortName = "GR75KG"; break;
            case "Greco-Roman 76kg": shortName = "GR76KG"; break;
            case "Greco-Roman 77kg": shortName = "GR77KG"; break;
            case "Greco-Roman 78kg": shortName = "GR78KG"; break;
            case "Greco-Roman 79kg": shortName = "GR79KG"; break;
            case "Greco-Roman 82.5kg": shortName = "GR82.5KG"; break;
            case "Greco-Roman 82kg": shortName = "GR82KG"; break;
            case "Greco-Roman 84kg": shortName = "GR84KG"; break;
            case "Greco-Roman 85kg": shortName = "GR85KG"; break;
            case "Greco-Roman 87kg": shortName = "GR87KG"; break;
            case "Greco-Roman 90kg": shortName = "GR90KG"; break;
            case "Greco-Roman 93kg": shortName = "GR93KG"; break;
            case "Greco-Roman 96kg": shortName = "GR96KG"; break;
            case "Greco-Roman 97kg": shortName = "GR97KG"; break;
            case "Greco-Roman 98kg": shortName = "GR98KG"; break;
            case "Greco-Roman Unlimited Class Greco-Roman": shortName = "GRO"; break;
            case "Group": shortName = "GROUP"; break;
            case "Halfpipe": shortName = "HALFPIPE"; break;
            case "Hammer Throw": shortName = "HAMMER"; break;
            case "Handball": shortName = "TEAM"; break;
            case "Heptathlon": shortName = "HEPTATH"; break;
            case "High Jump": shortName = "HJ"; break;
            case "Hockey": shortName = "TEAM"; break;
            case "Horizontal Bar": shortName = "HB"; break;
            case "Ice Dancing": shortName = "ICEDANCE"; break;
            case "Ice Hockey": shortName = "TEAM"; break;
            case "Individual": shortName = "INDIVID"; break;
            case "Individual 200m Medley": shortName = "200MIM"; break;
            case "Individual 400m Medley": shortName = "400MIM"; break;
            case "Individual All-Around": shortName = "AA"; break;
            case "Individual All-Around Field Sports": shortName = "AAFS"; break;
            case "Individual Cross-Country": shortName = "INDIVIDCC"; break;
            case "Individual Dressage": shortName = "DRESSINDV"; break;
            case "Individual Épée": shortName = "EPEE"; break;
            case "Individual Épée Masters": shortName = "EPEEM"; break;
            case "Individual Épée Masters And Amateurs": shortName = "EPEEMA"; break;
            case "Individual Eventing": shortName = "EVENTINDV"; break;
            case "Individual Fixed Large Bird": shortName = "FLB"; break;
            case "Individual Fixed Small Bird": shortName = "FSB"; break;
            case "Individual Foil": shortName = "FOIL"; break;
            case "Individual Foil Masters": shortName = "FOILM"; break;
            case "Individual Jumping": shortName = "JUMPINDV"; break;
            case "Individual Large Hill": shortName = "LH"; break;
            case "Individual Large Hill/10km": shortName = "LH10KM"; break;
            case "Individual Moving Bird 28m": shortName = "MB28M"; break;
            case "Individual Moving Bird 33m": shortName = "MB33M"; break;
            case "Individual Moving Bird 50m": shortName = "MB50M"; break;
            case "Individual Normal Hill": shortName = "NH"; break;
            case "Individual Normal Hill/10km": shortName = "NH10KM"; break;
            case "Individual Pursuit 3000m": shortName = "3000MPUR"; break;
            case "Individual Pursuit 4000m": shortName = "4000MPUR"; break;
            case "Individual Road Race": shortName = "RR"; break;
            case "Individual Sabre": shortName = "SABRE"; break;
            case "Individual Sabre Masters": shortName = "SABREM"; break;
            case "Individual Single Sticks": shortName = "SS"; break;
            case "Individual Time Trial": shortName = "TT"; break;
            case "Individual Vaulting": shortName = "VAULTINDV"; break;
            case "Javelin Throw": shortName = "JAVELIN"; break;
            case "Javelin Throw Both Hands": shortName = "JAVELINBH"; break;
            case "Javelin Throw Freestyle": shortName = "JAVELINFR"; break;
            case "Jumping High Jump": shortName = "JUMPHJ"; break;
            case "Jumping Long Jump": shortName = "JUMPLG"; break;
            case "K-1 10000m": shortName = "K1-10000M"; break;
            case "K-1 1000m": shortName = "K1-1000M"; break;
            case "K-1 200m": shortName = "K1-200M"; break;
            case "K-1 4x500m": shortName = "K1-4X500M"; break;
            case "K-1 500m": shortName = "K1-500M"; break;
            case "K-1 Slalom": shortName = "SLALOM"; break;
            case "K-2 10000m": shortName = "K2-10000M"; break;
            case "K-2 1000m": shortName = "K2-1000M"; break;
            case "K-2 200m": shortName = "K2-200M"; break;
            case "K-2 500m": shortName = "K2-500M"; break;
            case "K-4 1000m": shortName = "K4-1000M"; break;
            case "K-4 500m": shortName = "K4-500M"; break;
            case "Kata": shortName = "KATA"; break;
            case "Keirin": shortName = "KEIRIN"; break;
            case "Kumite +61 Kg": shortName = "O61KG"; break;
            case "Kumite +75 Kg": shortName = "O75KG"; break;
            case "Kumite -55 Kg": shortName = "55KG"; break;
            case "Kumite -61 Kg": shortName = "61KG"; break;
            case "Kumite -67 Kg": shortName = "67KG"; break;
            case "Kumite -75 Kg": shortName = "75KG"; break;
            case "Lacrosse": shortName = "LACROSSE"; break;
            case "Lightweight Coxless Four": shortName = "SCULL4-L"; break;
            case "Lightweight Double Sculls": shortName = "SCULL2-L"; break;
            case "Long Jump": shortName = "LONGJUMP"; break;
            case "Madison": shortName = "MADISON"; break;
            case "Marathon": shortName = "MARATHON"; break;
            case "Mass Start": shortName = "MASSSTART"; break;
            case "Military Pistol 25m": shortName = "MP25M"; break;
            case "Military Pistol 30m": shortName = "MP30M"; break;
            case "Military Rifle 200m": shortName = "MR200M"; break;
            case "Military Rifle Any Position 600m": shortName = "MRAP600M"; break;
            case "Military Rifle Prone 300m": shortName = "MRP300M"; break;
            case "Military Rifle Prone 600m": shortName = "MRP600M"; break;
            case "Military Rifle Standing 300m": shortName = "MRS300M"; break;
            case "Military Rifle Three Positions 300m": shortName = "MR3P300M"; break;
            case "Military Ski Patrol": shortName = "MSP"; break;
            case "Moguls": shortName = "MOGULS"; break;
            case "Monobob": shortName = "MONO"; break;
            case "Multihull": shortName = "MULTIHULL"; break;
            case "Muzzle-Loading Pistol 25m": shortName = "MLP25M"; break;
            case "Olympic Distance": shortName = "OD"; break;
            case "Omnium": shortName = "OMNIUM"; break;
            case "One Person Dinghy": shortName = "1PDINGHY"; break;
            case "One Person Heavyweight Dinghy": shortName = "1PHDINGHY"; break;
            case "Open": shortName = "OPEN"; break;
            case "Open Class": shortName = "OPENCLASS"; break;
            case "Pairs": shortName = "PAIRS"; break;
            case "Parallel Bars": shortName = "PB"; break;
            case "Parallel Giant Slalom": shortName = "PGS"; break;
            case "Parallel Slalom": shortName = "PSLALOM"; break;
            case "Park": shortName = "PARK"; break;
            case "Pentathlon": shortName = "PENTATH"; break;
            case "Plain High": shortName = "PH"; break;
            case "Platform": shortName = "10M"; break;
            case "Plunge For Distance": shortName = "PD"; break;
            case "Points Race": shortName = "POINTS"; break;
            case "Pole Vault": shortName = "POLEVAULT"; break;
            case "Polo": shortName = "TEAM"; break;
            case "Pommel Horse": shortName = "PH"; break;
            case "Quadruple Sculls": shortName = "SCULL4"; break;
            case "Rapid-Fire Pistol 25m": shortName = "RFP"; break;
            case "Relay": shortName = "RELAY"; break;
            case "Rings": shortName = "RINGS"; break;
            case "Rope Climbing": shortName = "RC"; break;
            case "Rugby": shortName = "TEAM"; break;
            case "Rugby Sevens": shortName = "TEAM"; break;
            case "Running Target 10m": shortName = "RT10M"; break;
            case "Running Target 50m": shortName = "RT50M"; break;
            case "Running Target Double Shot": shortName = "RTDS"; break;
            case "Running Target Single And Double Shot": shortName = "RTSDS"; break;
            case "Running Target Single Shot": shortName = "RTSS"; break;
            case "Shortboard": shortName = "SHORT"; break;
            case "Shot Put": shortName = "SHOTPUT"; break;
            case "Shot Put Both Hands": shortName = "SHOTPUTBH"; break;
            case "Side Horse": shortName = "SH"; break;
            case "Side Vault": shortName = "SV"; break;
            case "Single Sculls": shortName = "SCULL1"; break;
            case "Singles": shortName = "SINGLES"; break;
            case "Singles Indoor": shortName = "SINGLESIN"; break;
            case "Singles One Ball": shortName = "SINGLES1B"; break;
            case "Singles Two Balls": shortName = "SINGLES2B"; break;
            case "Skeet": shortName = "SKEET"; break;
            case "Skeleton": shortName = "SKELETON"; break;
            case "Ski Cross": shortName = "SKICROSS"; break;
            case "Skiff": shortName = "SKIFF"; break;
            case "Slalom": shortName = "SLALOM"; break;
            case "Slopestyle": shortName = "SLOPEST"; break;
            case "Small-Bore Rifle Any Position 50m": shortName = "SBRAP50M"; break;
            case "Small-Bore Rifle Disappearing Target 25 Yards": shortName = "SBRDT25Y"; break;
            case "Small-Bore Rifle Disappearing Target 25m": shortName = "SBRDT25M"; break;
            case "Small-Bore Rifle Moving Target 25 Yards": shortName = "SBRMT25Y"; break;
            case "Small-Bore Rifle Prone 50 And 100 Yards": shortName = "SBRP50100Y"; break;
            case "Small-Bore Rifle Prone 50m": shortName = "SBRP50M"; break;
            case "Small-Bore Rifle Standing 50m": shortName = "SBRS50M"; break;
            case "Small-Bore Rifle Three Positions 50m": shortName = "SBR3P50M"; break;
            case "Softball": shortName = "TEAM"; break;
            case "Solo": shortName = "SOLO"; break;
            case "Special Figures": shortName = "SFIGURES"; break;
            case "Sporting Pistol 25m": shortName = "SP25M"; break;
            case "Springboard": shortName = "3M"; break;
            case "Sprint": shortName = "SPRINT"; break;
            case "Sprint 1000m": shortName = "1000MSPRT"; break;
            case "Sprint 660 Yards": shortName = "660YSPRT"; break;
            case "Standing High Jump": shortName = "SHIGHJUMP"; break;
            case "Standing Long Jump": shortName = "SLONGJUMP"; break;
            case "Standing Triple Jump": shortName = "STRPLJUMP"; break;
            case "Street": shortName = "STREET"; break;
            case "Super G": shortName = "SUPERG"; break;
            case "Sur La Perche À La Herse": shortName = "SLPALH"; break;
            case "Sur La Perche À La Pyramide": shortName = "SLPALP"; break;
            case "Synchronized Platform": shortName = "TEAM10M"; break;
            case "Synchronized Springboard": shortName = "TEAM3M"; break;
            case "Tandem Sprint 2000m": shortName = "SP2000M"; break;
            case "Team": shortName = "TEAM"; break;
            case "Team 100km Time Trial": shortName = "T100KMTT"; break;
            case "Team 200m Swimming": shortName = "TEAM200M"; break;
            case "Team 3000m": shortName = "TEAM3000M"; break;
            case "Team 3miles": shortName = "TEAM3MILE"; break;
            case "Team 4miles": shortName = "TEAM4MILE"; break;
            case "Team 5000m": shortName = "TEAM5000M"; break;
            case "Team Aerials": shortName = "TEAMAERIAL"; break;
            case "Team Air Pistol 10m": shortName = "TEAMAP10M"; break;
            case "Team Air Rifle 10m": shortName = "TEAMAR10M"; break;
            case "Team All-Around": shortName = "TEAMAA"; break;
            case "Team All-Around Free System": shortName = "TEAMAAFS"; break;
            case "Team All-Around Swedish System": shortName = "TEAMAASS"; break;
            case "Team Cross": shortName = "TEAMCROSS"; break;
            case "Team Cross-Country": shortName = "TEAMCC"; break;
            case "Team Dressage": shortName = "DRESSTEAM"; break;
            case "Team Dueling Pistol 30m": shortName = "TEAMDP30M"; break;
            case "Team Épée": shortName = "TEAMEPEE"; break;
            case "Team Eventing": shortName = "EVENTTEAM"; break;
            case "Team Fixed Large Bird": shortName = "TEAMFLB"; break;
            case "Team Fixed Small Bird": shortName = "TEAMFSB"; break;
            case "Team Foil": shortName = "TEAMFOIL"; break;
            case "Team Free Pistol 50 Yards": shortName = "TEAMFP50Y"; break;
            case "Team Free Pistol 50m": shortName = "TEAMFP50M"; break;
            case "Team Free Rifle 400 600 And 800m": shortName = "TEAMFR400M"; break;
            case "Team Free Rifle Three Positions 300m": shortName = "TEAMFR3P"; break;
            case "Team Horizontal Bar": shortName = "TEAMHB"; break;
            case "Team Jumping": shortName = "JUMPTEAM"; break;
            case "Team Large Hill": shortName = "TEAMLH"; break;
            case "Team Military Pistol": shortName = "TEAMMP"; break;
            case "Team Military Rifle 200 400 500 And 600m": shortName = "TEAMMR200M"; break;
            case "Team Military Rifle 200/500/600/800/900/1000 Yards": shortName = "TEAMMR200Y"; break;
            case "Team Military Rifle Prone 300 And 600m": shortName = "TEAMMRP1"; break;
            case "Team Military Rifle Prone 300m": shortName = "TEAMMRP2"; break;
            case "Team Military Rifle Prone 600m": shortName = "TEAMMRP3"; break;
            case "Team Military Rifle Standing 300m": shortName = "TEAMMRS"; break;
            case "Team Moving Bird 28m": shortName = "TEAMMB28M"; break;
            case "Team Moving Bird 33m": shortName = "TEAMMB33M"; break;
            case "Team Moving Bird 50m": shortName = "TEAMMB50M"; break;
            case "Team Normal Hill": shortName = "TEAMNH"; break;
            case "Team Parallel Bars": shortName = "TEAMPB"; break;
            case "Team Portable Apparatus": shortName = "TEAMPA"; break;
            case "Team Pursuit": shortName = "TEAMPUR"; break;
            case "Team Pursuit 1980 Yards": shortName = "TEAMPUR2Y"; break;
            case "Team Pursuit 3000m": shortName = "TEAMPUR3KM"; break;
            case "Team Pursuit 4000m": shortName = "TEAMPUR4KM"; break;
            case "Team Relay": shortName = "TEAMRELAY"; break;
            case "Team Road Race": shortName = "TEAMRR"; break;
            case "Team Round": shortName = "TEAMROUND"; break;
            case "Team Running Target Double Shot": shortName = "TEAMRTDS"; break;
            case "Team Running Target Single Shot": shortName = "TEEAMRTSS"; break;
            case "Team Sabre": shortName = "TEAMSABRE"; break;
            case "Team Small-Bore Rifle 50 And 100 Yards": shortName = "TEAMSBR50Y"; break;
            case "Team Small-Bore Rifle Disappearing Target 25m": shortName = "TEAMSBRDT"; break;
            case "Team Small-Bore Rifle Prone 50m": shortName = "TEAMSBRP50"; break;
            case "Team Small-Bore Rifle Standing 50m": shortName = "TEAMSNBRS"; break;
            case "Team Sprint": shortName = "TEAMSPRINT"; break;
            case "Team Trap": shortName = "TEAMTRAP"; break;
            case "Team Two-Man With Cesta": shortName = "TEAM"; break;
            case "Team Vaulting": shortName = "VAULTTEAM"; break;
            case "Three Person Keelboat": shortName = "3PKEEL"; break;
            case "Three Person Keelboat Dragon": shortName = "3PKEELDR"; break;
            case "Three Person Keelboat Soling": shortName = "3PKEELSO"; break;
            case "Trap": shortName = "TRAP"; break;
            case "Triathlon": shortName = "TRI"; break;
            case "Triple Jump": shortName = "TRPLJUMP"; break;
            case "Tug-Of-War": shortName = "TUGOFWAR"; break;
            case "Tumbling": shortName = "TUMBLING"; break;
            case "Two": shortName = "TWO"; break;
            case "Two Person Dinghy": shortName = "2PDINGHY"; break;
            case "Two Person Heavyweight Dinghy": shortName = "2PHDINGHY"; break;
            case "Two Person Keelboat": shortName = "2PKEEL"; break;
            case "Two Person Keelboat Star": shortName = "2PKEELSTAR"; break;
            case "Two Person Keelboat Swallow": shortName = "2PKEELSWAL"; break;
            case "Two Person Keelboat Tempest": shortName = "2PKEELTEMP"; break;
            case "Underwater Swimming": shortName = "USWIM"; break;
            case "Uneven Bars": shortName = "UB"; break;
            case "Unknown Event": shortName = "UE"; break;
            case "Unlimited One Hand": shortName = "UOH"; break;
            case "Unlimited Two Hands": shortName = "UTH"; break;
            case "Vault": shortName = "VAULT"; break;
            case "Volleyball": shortName = "TEAM"; break;
            case "Water Polo": shortName = "TEAM"; break;
            case "Windsurfer": shortName = "WIND"; break;
        }

        return shortName;
    }

    public string CreateSEOName(string text)
    {
        return text.ToLower().Replace("'", " ").Replace(" ", "-");
    }

    public string MapPhase(string name)
    {
        var phase = string.Empty;

        switch (name)
        {
            case "1/8-Final Repêchage": phase = "8FNL"; break;
            case "1/8-Final Repêchage Final": phase = "8FNL"; break;
            case "Eighth-Finals": phase = "8FNL"; break;
            case "Cancelled Round": phase = "CCLR"; break;
            case "Consolation Final": phase = "CNR"; break;
            case "Consolation Round": phase = "CNR"; break;
            case "Consolation Round - Final": phase = "CNR"; break;
            case "Consolation Round - Round One": phase = "CNR"; break;
            case "Consolation Round - Semi-Finals": phase = "CNR"; break;
            case "Consolation Round: Final": phase = "CNR"; break;
            case "Consolation Round: Quarter-Finals": phase = "CNR"; break;
            case "Consolation Round: Semi-Finals": phase = "CNR"; break;
            case "Consolation Tournament": phase = "CNR"; break;
            case "Compulsory Dance": phase = "COMD"; break;
            case "Compulsory Dance 1": phase = "COMD"; break;
            case "Compulsory Dance 2": phase = "COMD"; break;
            case "Compulsory Dances": phase = "COMD"; break;
            case "Compulsory Figures": phase = "COMF"; break;
            case "Classification Round": phase = "CR"; break;
            case "Classification Round 13-15": phase = "CR"; break;
            case "Classification Round 13-16": phase = "CR"; break;
            case "Classification Round 17-20": phase = "CR"; break;
            case "Classification Round 17-23": phase = "CR"; break;
            case "Classification Round 21-23": phase = "CR"; break;
            case "Classification Round 2-3": phase = "CR"; break;
            case "Classification Round 3rd Place": phase = "CR"; break;
            case "Classification Round 5-11": phase = "CR"; break;
            case "Classification Round 5-8": phase = "CR"; break;
            case "Classification Round 5-82": phase = "CR"; break;
            case "Classification Round 7-10": phase = "CR"; break;
            case "Classification Round 7-12": phase = "CR"; break;
            case "Classification Round 9-11": phase = "CR"; break;
            case "Classification Round 9-12": phase = "CR"; break;
            case "Classification Round 9-123": phase = "CR"; break;
            case "Classification Round 9-16": phase = "CR"; break;
            case "Classification Round Five": phase = "CR"; break;
            case "Classification Round for 5/6": phase = "CR"; break;
            case "Classification Round Four": phase = "CR"; break;
            case "Classification Round One": phase = "CR"; break;
            case "Classification Round Six": phase = "CR"; break;
            case "Classification Round Three": phase = "CR"; break;
            case "Classification Round Two": phase = "CR"; break;
            case "Elimination Round": phase = "ER"; break;
            case "Elimination Rounds": phase = "ER"; break;
            case "Elimination Rounds, Round Five Repêchage": phase = "ER"; break;
            case "Elimination Rounds, Round Four": phase = "ER"; break;
            case "Elimination Rounds, Round Four Repêchage": phase = "ER"; break;
            case "Elimination Rounds, Round One": phase = "ER"; break;
            case "Elimination Rounds, Round One Repêchage": phase = "ER"; break;
            case "Elimination Rounds, Round Three": phase = "ER"; break;
            case "Elimination Rounds, Round Three Repêchage": phase = "ER"; break;
            case "Elimination Rounds, Round Two": phase = "ER"; break;
            case "Elimination Rounds, Round Two Repêchage": phase = "ER"; break;
            case "Play-offs": phase = "ER"; break;
            case "Free Dance": phase = "FD"; break;
            case "Figures": phase = "FG"; break;
            case "Fleet Races": phase = "FL"; break;
            case "A Final": phase = "FNL"; break;
            case "B Final": phase = "FNL"; break;
            case "Barrage for 1/2": phase = "FNL"; break;
            case "Classification Final 1": phase = "FNL"; break;
            case "Classification Final 2": phase = "FNL"; break;
            case "Final": phase = "FNL"; break;
            case "Final A": phase = "FNL"; break;
            case "Final B": phase = "FNL"; break;
            case "Final C": phase = "FNL"; break;
            case "Final D": phase = "FNL"; break;
            case "Final E": phase = "FNL"; break;
            case "Final F": phase = "FNL"; break;
            case "Final Heat": phase = "FNL"; break;
            case "Final Heat One": phase = "FNL"; break;
            case "Final Heat Two": phase = "FNL"; break;
            case "Final Pool": phase = "FNL"; break;
            case "Final Pool Barrage 2-3": phase = "FNL"; break;
            case "Final Pool, Barrage #1 1-2": phase = "FNL"; break;
            case "Final Pool, Barrage #2 1-2": phase = "FNL"; break;
            case "Final Pool, Barrage 1-2": phase = "FNL"; break;
            case "Final Pool, Barrage 1-3": phase = "FNL"; break;
            case "Final Pool, Barrage 1-4": phase = "FNL"; break;
            case "Final Pool, Barrage 2-3": phase = "FNL"; break;
            case "Final Pool, Barrage 2-4": phase = "FNL"; break;
            case "Final Pool, Barrage 2-5": phase = "FNL"; break;
            case "Final Pool, Barrage 3-4": phase = "FNL"; break;
            case "Final Pool, Barrage 3-5": phase = "FNL"; break;
            case "Final Pool, Barrage 4-5": phase = "FNL"; break;
            case "Final Pool, Barrage 6-7": phase = "FNL"; break;
            case "Final Round": phase = "FNL"; break;
            case "Final Round 1": phase = "FNL"; break;
            case "Final Round 2": phase = "FNL"; break;
            case "Final Round 3": phase = "FNL"; break;
            case "Final Round One": phase = "FNL"; break;
            case "Final Round Three": phase = "FNL"; break;
            case "Final Round Two": phase = "FNL"; break;
            case "Final Round2": phase = "FNL"; break;
            case "Final, Swim-Off": phase = "FNL"; break;
            case "Final1": phase = "FNL"; break;
            case "First Final": phase = "FNL"; break;
            case "Match 1/2": phase = "FNL"; break;
            case "Match 1-6": phase = "FNL"; break;
            case "Match 3/4": phase = "FNL"; break;
            case "Medal Pool": phase = "FNL"; break;
            case "Original Final": phase = "FNL"; break;
            case "Play-Off for Bronze Medal": phase = "FNL"; break;
            case "Play-Off for Silver Medal": phase = "FNL"; break;
            case "Second Place Tournament - Final": phase = "FNL"; break;
            case "Third-Place Tournament": phase = "FNL"; break;
            case "Free Skating": phase = "FS"; break;
            case "Grand Prix": phase = "GP"; break;
            case "Round-Robin": phase = "GP"; break;
            case "Group A": phase = "GPA"; break;
            case "Group A - Final": phase = "GPA"; break;
            case "Group A - Round Five": phase = "GPA"; break;
            case "Group A - Round Four": phase = "GPA"; break;
            case "Group A - Round One": phase = "GPA"; break;
            case "Group A - Round Seven": phase = "GPA"; break;
            case "Group A - Round Six": phase = "GPA"; break;
            case "Group A - Round Three": phase = "GPA"; break;
            case "Group A - Round Two": phase = "GPA"; break;
            case "Group A1": phase = "GPA"; break;
            case "Group B": phase = "GPB"; break;
            case "Group B - Final": phase = "GPB"; break;
            case "Group B - Round Five": phase = "GPB"; break;
            case "Group B - Round Four": phase = "GPB"; break;
            case "Group B - Round One": phase = "GPB"; break;
            case "Group B - Round Seven": phase = "GPB"; break;
            case "Group B - Round Six": phase = "GPB"; break;
            case "Group B - Round Three": phase = "GPB"; break;
            case "Group B - Round Two": phase = "GPB"; break;
            case "Group B2": phase = "GPB"; break;
            case "Group C": phase = "GPC"; break;
            case "Group C3": phase = "GPC"; break;
            case "Group D": phase = "GPD"; break;
            case "Group E": phase = "GPE"; break;
            case "Grand Prix Freestyle": phase = "GPF"; break;
            case "Group F": phase = "GPF"; break;
            case "Group G": phase = "GPG"; break;
            case "Group H": phase = "GPH"; break;
            case "Group I": phase = "GPI"; break;
            case "Group J": phase = "GPJ"; break;
            case "Group K": phase = "GPK"; break;
            case "Group L": phase = "GPL"; break;
            case "Group M": phase = "GPM"; break;
            case "Group N": phase = "GPN"; break;
            case "Group O": phase = "GPO"; break;
            case "Grand Prix Special": phase = "GPS"; break;
            case "Round Robin": phase = "GR"; break;
            case "Group One": phase = "GRA"; break;
            case "Pool 1": phase = "GRA"; break;
            case "Pool 1, Barrage": phase = "GRA"; break;
            case "Pool 1, Barrage 2-5": phase = "GRA"; break;
            case "Pool 1, Barrage 3-4": phase = "GRA"; break;
            case "Pool 1, Barrage 3-5": phase = "GRA"; break;
            case "Pool 1, Barrage 3-6": phase = "GRA"; break;
            case "Pool 1, Barrage 4-5": phase = "GRA"; break;
            case "Pool 1, Barrage 4-6": phase = "GRA"; break;
            case "Pool 1, Barrage 6-8": phase = "GRA"; break;
            case "Pool A": phase = "GRA"; break;
            case "Pool One": phase = "GRA"; break;
            case "Round One Pool One": phase = "GRA"; break;
            case "Group Two": phase = "GRB"; break;
            case "Pool 2": phase = "GRB"; break;
            case "Pool 2, Barrage 2-4": phase = "GRB"; break;
            case "Pool 2, Barrage 3-4": phase = "GRB"; break;
            case "Pool 2, Barrage 3-5": phase = "GRB"; break;
            case "Pool 2, Barrage 3-7": phase = "GRB"; break;
            case "Pool 2, Barrage 4-5": phase = "GRB"; break;
            case "Pool 2, Barrage 4-6": phase = "GRB"; break;
            case "Pool 2, Barrage 5-6": phase = "GRB"; break;
            case "Pool 2, Barrage 5-8": phase = "GRB"; break;
            case "Pool 2, Barrage 6-12": phase = "GRB"; break;
            case "Pool B": phase = "GRB"; break;
            case "Pool Two": phase = "GRB"; break;
            case "Round One Pool Two": phase = "GRB"; break;
            case "Pool 3": phase = "GRC"; break;
            case "Pool 3, Barrage 3-5": phase = "GRC"; break;
            case "Pool 3, Barrage 4-5": phase = "GRC"; break;
            case "Pool 3, Barrage 4-6": phase = "GRC"; break;
            case "Pool 3, Barrage 5-6": phase = "GRC"; break;
            case "Pool 3, Barrage 6-8": phase = "GRC"; break;
            case "Pool C": phase = "GRC"; break;
            case "Pool Three": phase = "GRC"; break;
            case "Round One Pool Three": phase = "GRC"; break;
            case "Pool 4": phase = "GRD"; break;
            case "Pool 4, Barrage": phase = "GRD"; break;
            case "Pool 4, Barrage 2-4": phase = "GRD"; break;
            case "Pool 4, Barrage 2-5": phase = "GRD"; break;
            case "Pool 4, Barrage 3-4": phase = "GRD"; break;
            case "Pool 4, Barrage 3-5": phase = "GRD"; break;
            case "Pool 4, Barrage 4-5": phase = "GRD"; break;
            case "Pool 4, Barrage 4-6": phase = "GRD"; break;
            case "Pool 4, Barrage 6-8": phase = "GRD"; break;
            case "Pool D": phase = "GRD"; break;
            case "Pool Four": phase = "GRD"; break;
            case "Round One Pool Four": phase = "GRD"; break;
            case "Pool 5": phase = "GRE"; break;
            case "Pool 5, Barrage 2-4": phase = "GRE"; break;
            case "Pool 5, Barrage 3-4": phase = "GRE"; break;
            case "Pool 5, Barrage 3-6": phase = "GRE"; break;
            case "Pool 5, Barrage 4-6": phase = "GRE"; break;
            case "Pool 5, Barrage 5-7": phase = "GRE"; break;
            case "Pool E": phase = "GRE"; break;
            case "Pool Five": phase = "GRE"; break;
            case "Round One Pool Five": phase = "GRE"; break;
            case "Pool 6": phase = "GRF"; break;
            case "Pool 6, Barrage": phase = "GRF"; break;
            case "Pool 6, Barrage 3-4": phase = "GRF"; break;
            case "Pool 6, Barrage 3-5": phase = "GRF"; break;
            case "Pool 6, Barrage 4-5": phase = "GRF"; break;
            case "Pool 6, Barrage 5-6": phase = "GRF"; break;
            case "Pool F": phase = "GRF"; break;
            case "Round One Pool Six": phase = "GRF"; break;
            case "Pool 7": phase = "GRG"; break;
            case "Pool 7, Barrage 2-4": phase = "GRG"; break;
            case "Pool 7, Barrage 3-5": phase = "GRG"; break;
            case "Pool 7, Barrage 4-6": phase = "GRG"; break;
            case "Pool G": phase = "GRG"; break;
            case "Pool 8": phase = "GRH"; break;
            case "Pool 8, Barrage 2-4": phase = "GRH"; break;
            case "Pool 8, Barrage 3-4": phase = "GRH"; break;
            case "Pool 8, Barrage 3-5": phase = "GRH"; break;
            case "Pool 8, Barrage 4-5": phase = "GRH"; break;
            case "Pool H": phase = "GRH"; break;
            case "Pool 9": phase = "GRI"; break;
            case "Pool 10": phase = "GRJ"; break;
            case "Pool 10, Barrage 2-4": phase = "GRJ"; break;
            case "Pool 10, Barrage 3-4": phase = "GRJ"; break;
            case "Pool 11": phase = "GRK"; break;
            case "Pool 11, Barrage 2-4": phase = "GRK"; break;
            case "Pool 11, Barrage 3-5": phase = "GRK"; break;
            case "Pool 12": phase = "GRL"; break;
            case "Pool 12, Barrage 2-4": phase = "GRL"; break;
            case "Pool 12, Barrage 3-4": phase = "GRL"; break;
            case "Pool 13": phase = "GRM"; break;
            case "Pool 14": phase = "GRN"; break;
            case "Pool 15": phase = "GRO"; break;
            case "Group P": phase = "GRP"; break;
            case "Pool 16": phase = "GRP"; break;
            case "Pool 17": phase = "GRQ"; break;
            case "Jump-Off": phase = "JO"; break;
            case "Jump-Off for 1-2": phase = "JO"; break;
            case "Jump-off for 2-4": phase = "JO"; break;
            case "Jump-Off for 3-4": phase = "JO"; break;
            case "Jump-off for 3-5": phase = "JO"; break;
            case "Jump-Off for 3-9": phase = "JO"; break;
            case "Jump-off for 6-7": phase = "JO"; break;
            case "Lucky Loser Round": phase = "LL"; break;
            case "Original Set Pattern Dance": phase = "OSPD"; break;
            case "Heat #1": phase = "PREM"; break;
            case "Heat #1 Re-Race": phase = "PREM"; break;
            case "Heat #10": phase = "PREM"; break;
            case "Heat #11": phase = "PREM"; break;
            case "Heat #12": phase = "PREM"; break;
            case "Heat #13": phase = "PREM"; break;
            case "Heat #14": phase = "PREM"; break;
            case "Heat #15": phase = "PREM"; break;
            case "Heat #16": phase = "PREM"; break;
            case "Heat #17": phase = "PREM"; break;
            case "Heat #2": phase = "PREM"; break;
            case "Heat #3": phase = "PREM"; break;
            case "Heat #4": phase = "PREM"; break;
            case "Heat #5": phase = "PREM"; break;
            case "Heat #6": phase = "PREM"; break;
            case "Heat #7": phase = "PREM"; break;
            case "Heat #8": phase = "PREM"; break;
            case "Heat #9": phase = "PREM"; break;
            case "Heat 1/2": phase = "PREM"; break;
            case "Heat 1-6": phase = "PREM"; break;
            case "Heat 3/4": phase = "PREM"; break;
            case "Heat 5/6": phase = "PREM"; break;
            case "Heat 5-8": phase = "PREM"; break;
            case "Heat 7/8": phase = "PREM"; break;
            case "Heat 7-12": phase = "PREM"; break;
            case "Heat 9-12": phase = "PREM"; break;
            case "Heat Eight": phase = "PREM"; break;
            case "Heat Eighteen": phase = "PREM"; break;
            case "Heat Eleven": phase = "PREM"; break;
            case "Heat Fifteen": phase = "PREM"; break;
            case "Heat Five": phase = "PREM"; break;
            case "Heat Four": phase = "PREM"; break;
            case "Heat Fourteen": phase = "PREM"; break;
            case "Heat Nine": phase = "PREM"; break;
            case "Heat One": phase = "PREM"; break;
            case "Heat One Re-Run": phase = "PREM"; break;
            case "Heat Seven": phase = "PREM"; break;
            case "Heat Seventeen": phase = "PREM"; break;
            case "Heat Six": phase = "PREM"; break;
            case "Heat Six Re-Run": phase = "PREM"; break;
            case "Heat Sixteen": phase = "PREM"; break;
            case "Heat Ten": phase = "PREM"; break;
            case "Heat Thirteen": phase = "PREM"; break;
            case "Heat Three": phase = "PREM"; break;
            case "Heat Three Re-run": phase = "PREM"; break;
            case "Heat Twelve": phase = "PREM"; break;
            case "Heat Two": phase = "PREM"; break;
            case "Heat Two Re-run": phase = "PREM"; break;
            case "Preliminary Round": phase = "PREM"; break;
            case "Round One, Heat Ten": phase = "PREM"; break;
            case "Prime #1": phase = "PRIM"; break;
            case "Prime #10": phase = "PRIM"; break;
            case "Prime #2": phase = "PRIM"; break;
            case "Prime #3": phase = "PRIM"; break;
            case "Prime #4": phase = "PRIM"; break;
            case "Prime #5": phase = "PRIM"; break;
            case "Prime #6": phase = "PRIM"; break;
            case "Prime #7": phase = "PRIM"; break;
            case "Prime #8": phase = "PRIM"; break;
            case "Prime #9": phase = "PRIM"; break;
            case "Precision Section": phase = "PS"; break;
            case "Quarter Finals": phase = "QFNL"; break;
            case "Quarter-Finals": phase = "QFNL"; break;
            case "Quarter-Finals Repêchage": phase = "QFNL"; break;
            case "Quarter-Finals, 64032": phase = "QFNL"; break;
            case "Qualification": phase = "QUAL"; break;
            case "Qualification Round": phase = "QUAL"; break;
            case "Qualifying": phase = "QUAL"; break;
            case "Qualifying Round": phase = "QUAL"; break;
            case "Qualifying Round 1": phase = "QUAL"; break;
            case "Qualifying Round 2": phase = "QUAL"; break;
            case "Qualifying Round One": phase = "QUAL"; break;
            case "Qualifying Round Two": phase = "QUAL"; break;
            case "Qualifying Round, Group A": phase = "QUAL"; break;
            case "Qualifying Round, Group A Re-Jump": phase = "QUAL"; break;
            case "Qualifying Round, Group A1": phase = "QUAL"; break;
            case "Qualifying Round, Group B": phase = "QUAL"; break;
            case "Qualifying Round, Group B1": phase = "QUAL"; break;
            case "Qualifying Round, Group C": phase = "QUAL"; break;
            case "Qualifying Round, Group C3": phase = "QUAL"; break;
            case "Qualifying Round, Group D": phase = "QUAL"; break;
            case "Qualifying Round, Group D4": phase = "QUAL"; break;
            case "Qualifying Round, Group E": phase = "QUAL"; break;
            case "Qualifying Round, Group F": phase = "QUAL"; break;
            case "Qualifying Round, Group One": phase = "QUAL"; break;
            case "Qualifying Round, Group Two": phase = "QUAL"; break;
            case "Ranking Round": phase = "QUAL"; break;
            case "Rankings": phase = "QUAL"; break;
            case "Seeding Round": phase = "QUAL"; break;
            case "Race Eight": phase = "RACE"; break;
            case "Race Five": phase = "RACE"; break;
            case "Race Four": phase = "RACE"; break;
            case "Race Nine": phase = "RACE"; break;
            case "Race One": phase = "RACE"; break;
            case "Race Seven": phase = "RACE"; break;
            case "Race Six": phase = "RACE"; break;
            case "Race Ten": phase = "RACE"; break;
            case "Race Three": phase = "RACE"; break;
            case "Race Two": phase = "RACE"; break;
            case "Rhythm Dance": phase = "RD"; break;
            case "Repêchage": phase = "REP"; break;
            case "Repêchage Heats": phase = "REP"; break;
            case "Re-run Final": phase = "REP"; break;
            case "Re-run of Heat Two": phase = "REP"; break;
            case "Round One Repêchage": phase = "REP"; break;
            case "Round Three Repêchage": phase = "REP"; break;
            case "Round Two Repêchage": phase = "REP"; break;
            case "Repêchage Round One": phase = "REP1"; break;
            case "Second Place Tournament - Round One": phase = "REP1"; break;
            case "Repêchage Round Two": phase = "REP2"; break;
            case "Second Place Tournament - Round Two": phase = "REP2"; break;
            case "Second Place Tournament - Semi-Finals": phase = "REP3"; break;
            case "2nd-Place Final Round": phase = "REPF"; break;
            case "2nd-Place Round One": phase = "REPF"; break;
            case "2nd-Place Semi-Finals": phase = "REPF"; break;
            case "2nd-Place Tournament": phase = "REPF"; break;
            case "3rd-Place Final Round": phase = "REPF"; break;
            case "3rd-Place Quarter-Finals": phase = "REPF"; break;
            case "3rd-Place Round One": phase = "REPF"; break;
            case "3rd-Place Semi-Finals": phase = "REPF"; break;
            case "3rd-Place Tournament": phase = "REPF"; break;
            case "Repêchage Final": phase = "REPF"; break;
            case "Round One Repêchage Final": phase = "REPF"; break;
            case "Round Two Repêchage Final": phase = "REPF"; break;
            case "Second-Place Tournament": phase = "REPF"; break;
            case "Second-to-Fifth Place Tournament": phase = "REPF"; break;
            case "Original Round One": phase = "RND1"; break;
            case "Round One": phase = "RND1"; break;
            case "Round One Rerace": phase = "RND1"; break;
            case "Round One1": phase = "RND1"; break;
            case "Round One9": phase = "RND1"; break;
            case "Round Two": phase = "RND2"; break;
            case "Round Three": phase = "RND3"; break;
            case "Round Four": phase = "RND4"; break;
            case "Round Four5": phase = "RND4"; break;
            case "Round Five": phase = "RND5"; break;
            case "Round Six": phase = "RND6"; break;
            case "Round Seven": phase = "RND7"; break;
            case "Short Dance": phase = "SD"; break;
            case "Classification 5-8": phase = "SF5"; break;
            case "Match 5-7": phase = "SF5"; break;
            case "Match 5-8": phase = "SF5"; break;
            case "Match 7-10": phase = "SF7"; break;
            case "Classification 9-12": phase = "SF9"; break;
            case "Match 9-12": phase = "SF9"; break;
            case "Semi-Final": phase = "SFNL"; break;
            case "Semi-Final Round": phase = "SFNL"; break;
            case "Semi-Finals": phase = "SFNL"; break;
            case "Semi-Finals A/B": phase = "SFNL"; break;
            case "Semi-Finals C/D": phase = "SFNL"; break;
            case "Semi-Finals E/F": phase = "SFNL"; break;
            case "Semi-Finals Repêchage": phase = "SFNL"; break;
            case "Semi-Finals3": phase = "SFNL"; break;
            case "Shoot-Off": phase = "SO"; break;
            case "Shoot-Off 1": phase = "SO"; break;
            case "Shoot-Off 2": phase = "SO"; break;
            case "Shoot-Off for 1st Place": phase = "SO"; break;
            case "Shoot-Off for 2nd Place": phase = "SO"; break;
            case "Shoot-Off for 3rd Place": phase = "SO"; break;
            case "Swim-Off": phase = "SO"; break;
            case "Swim-Off for 16th Place": phase = "SO"; break;
            case "Swim-Off for 16th Place - Race 1": phase = "SO"; break;
            case "Swim-Off for 16th Place - Race 2": phase = "SO"; break;
            case "Swim-Off for 8th Place": phase = "SO"; break;
            case "Swim-Off for Places 7-8": phase = "SO"; break;
            case "Short Program": phase = "SP"; break;
            case "Tie-Breaker": phase = "TB"; break;

        }

        return phase;
    }
}