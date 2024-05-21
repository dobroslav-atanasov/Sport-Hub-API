namespace SportData.Services;

using System.Text.RegularExpressions;

using SportData.Data.Models.Converters.OlympicGames;
using SportData.Data.Models.Entities.OlympicGames.Enumerations;
using SportData.Services.Interfaces;

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

    public string NormalizeEventName(string name, int gameYear, string disciplineName)
    {
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
            //.Replace("", "")
            //.Replace("", "")
            .Replace("Target Archery", "Moving Bird");

        if (gameYear == 1924 && disciplineName == "Artistic Gymnastics" && name == "Side Horse, Men")
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
            Type = RoundTypeEnum.None,
            SubType = RoundTypeEnum.None
        };

        switch (name)
        {
            case "Barrage for 1/2": round.Type = RoundTypeEnum.GoldMedalMatch; break;
            case "Classification 5-8": round.Type = RoundTypeEnum.Classification; round.Info = "5-8"; break;
            case "Classification 9-12": round.Type = RoundTypeEnum.Classification; round.Info = "9-12"; break;
            case "Classification Round": round.Type = RoundTypeEnum.Classification; break;
            case "Classification Round 13-15": round.Type = RoundTypeEnum.Classification; round.Info = "13-15"; break;
            case "Classification Round 13-16": round.Type = RoundTypeEnum.Classification; round.Info = "13-16"; break;
            case "Classification Round 17-20": round.Type = RoundTypeEnum.Classification; round.Info = "17-20"; break;
            case "Classification Round 17-23": round.Type = RoundTypeEnum.Classification; round.Info = "17-23"; break;
            case "Classification Round 21-23": round.Type = RoundTypeEnum.Classification; round.Info = "21-23"; break;
            case "Classification Round 2-3": round.Type = RoundTypeEnum.SilverMedalMatch; break;
            case "Classification Round 3rd Place": round.Type = RoundTypeEnum.BronzeMedalMatch; break;
            case "Classification Round 5-11": round.Type = RoundTypeEnum.Classification; round.Info = "5-11"; break;
            case "Classification Round 5-8": round.Type = RoundTypeEnum.Classification; round.Info = "5-8"; break;
            case "Classification Round 5-82": round.Type = RoundTypeEnum.Classification; round.Info = "5-8"; break;
            case "Classification Round 7-10": round.Type = RoundTypeEnum.Classification; round.Info = "7-10"; break;
            case "Classification Round 7-12": round.Type = RoundTypeEnum.Classification; round.Info = "7-12"; break;
            case "Classification Round 9-11": round.Type = RoundTypeEnum.Classification; round.Info = "9-11"; break;
            case "Classification Round 9-12": round.Type = RoundTypeEnum.Classification; round.Info = "9-12"; break;
            case "Classification Round 9-123": round.Type = RoundTypeEnum.Classification; round.Info = "9-12"; break;
            case "Classification Round 9-16": round.Type = RoundTypeEnum.Classification; round.Info = "9-16"; break;
            case "Classification Round for 5/6": round.Type = RoundTypeEnum.Classification; round.Info = "5-6"; break;
            case "Preliminary Round": round.Type = RoundTypeEnum.PreliminaryRound; break;
            case "Quarter Finals": round.Type = RoundTypeEnum.Quarterfinals; break;
            case "Quarter-Finals": round.Type = RoundTypeEnum.Quarterfinals; break;
            case "Quarter-Finals, 64032": round.Type = RoundTypeEnum.Quarterfinals; break;
            case "Quarter-Finals Repêchage": round.Type = RoundTypeEnum.Quarterfinals; round.SubType = RoundTypeEnum.Repechage; break;
            case "Quarter-Finals Repechage": round.Type = RoundTypeEnum.Quarterfinals; round.SubType = RoundTypeEnum.Repechage; break;
            case "Semi-Final": round.Type = RoundTypeEnum.Semifinals; break;
            case "Semi-Final Round": round.Type = RoundTypeEnum.Semifinals; break;
            case "Semi-Finals": round.Type = RoundTypeEnum.Semifinals; break;
            case "Semi-Finals3": round.Type = RoundTypeEnum.Semifinals; break;
            case "Semi-Finals Repechage": round.Type = RoundTypeEnum.Semifinals; round.SubType = RoundTypeEnum.Repechage; break;
            case "Semi-Finals Repêchage": round.Type = RoundTypeEnum.Semifinals; round.SubType = RoundTypeEnum.Repechage; break;
            case "Semi-Finals A/B": round.Type = RoundTypeEnum.Semifinals; round.Info = "A-B"; break;
            case "Semi-Finals C/D": round.Type = RoundTypeEnum.Semifinals; round.Info = "C-D"; break;
            case "Semi-Finals E/F": round.Type = RoundTypeEnum.Semifinals; round.Info = "E-F"; break;
            case "1/8-Final Repechage": round.Type = RoundTypeEnum.Eightfinals; round.SubType = RoundTypeEnum.Repechage; break;
            case "1/8-Final Repechage Final": round.Type = RoundTypeEnum.Eightfinals; round.SubType = RoundTypeEnum.Repechage; break;
            case "1/8-Final Repêchage Final": round.Type = RoundTypeEnum.Eightfinals; round.SubType = RoundTypeEnum.Repechage; break;
            case "2nd-Place Final Round": round.Type = RoundTypeEnum.Repechage; round.SubType = RoundTypeEnum.SilverMedalMatch; break;
            case "2nd-Place Round One": round.Type = RoundTypeEnum.Repechage; round.SubType = RoundTypeEnum.SilverMedalMatch; break;
            case "2nd-Place Semi-Finals": round.Type = RoundTypeEnum.Repechage; round.SubType = RoundTypeEnum.SilverMedalMatch; break;
            case "2nd-Place Tournament": round.Type = RoundTypeEnum.Repechage; round.SubType = RoundTypeEnum.SilverMedalMatch; break;
            case "3rd-Place Final Round": round.Type = RoundTypeEnum.Repechage; round.SubType = RoundTypeEnum.BronzeMedalMatch; break;
            case "3rd-Place Quarter-Finals": round.Type = RoundTypeEnum.Repechage; round.SubType = RoundTypeEnum.BronzeMedalMatch; break;
            case "3rd-Place Round One": round.Type = RoundTypeEnum.Repechage; round.SubType = RoundTypeEnum.BronzeMedalMatch; break;
            case "3rd-Place Semi-Finals": round.Type = RoundTypeEnum.Repechage; round.SubType = RoundTypeEnum.BronzeMedalMatch; break;
            case "3rd-Place Tournament": round.Type = RoundTypeEnum.Repechage; round.SubType = RoundTypeEnum.BronzeMedalMatch; break;
            case "Group A": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Group; round.Number = 1; break;
            case "Group A - Final": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Group; round.Number = 1; break;
            case "Group A - Round Five": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Group; round.Number = 1; break;
            case "Group A - Round Four": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Group; round.Number = 1; break;
            case "Group A - Round One": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Group; round.Number = 1; break;
            case "Group A - Round Seven": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Group; round.Number = 1; break;
            case "Group A - Round Six": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Group; round.Number = 1; break;
            case "Group A - Round Three": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Group; round.Number = 1; break;
            case "Group A - Round Two": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Group; round.Number = 1; break;
            case "Group A1": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Group; round.Number = 1; break;
            case "Group B": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Group; round.Number = 2; break;
            case "Group B - Final": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Group; round.Number = 2; break;
            case "Group B - Round Five": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Group; round.Number = 2; break;
            case "Group B - Round Four": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Group; round.Number = 2; break;
            case "Group B - Round One": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Group; round.Number = 2; break;
            case "Group B - Round Seven": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Group; round.Number = 2; break;
            case "Group B - Round Six": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Group; round.Number = 2; break;
            case "Group B - Round Three": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Group; round.Number = 2; break;
            case "Group B - Round Two": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Group; round.Number = 2; break;
            case "Group B2": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Group; round.Number = 2; break;
            case "Group C": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Group; round.Number = 3; break;
            case "Group C3": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Group; round.Number = 3; break;
            case "Group D": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Group; round.Number = 4; break;
            case "Group E": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Group; round.Number = 5; break;
            case "Group F": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Group; round.Number = 6; break;
            case "Group G": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Group; round.Number = 7; break;
            case "Group H": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Group; round.Number = 8; break;
            case "Group I": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Group; round.Number = 9; break;
            case "Group J": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Group; round.Number = 10; break;
            case "Group K": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Group; round.Number = 11; break;
            case "Group L": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Group; round.Number = 12; break;
            case "Group M": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Group; round.Number = 13; break;
            case "Group N": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Group; round.Number = 14; break;
            case "Group O": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Group; round.Number = 15; break;
            case "Group One": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Group; round.Number = 1; break;
            case "Group P": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Group; round.Number = 16; break;
            case "Group Two": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Group; round.Number = 2; break;
            case "Heat #1": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Heat; round.Number = 1; break;
            case "Heat #1 Re-Race": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Heat; round.Number = 1; break;
            case "Heat #10": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Heat; round.Number = 10; break;
            case "Heat #11": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Heat; round.Number = 11; break;
            case "Heat #12": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Heat; round.Number = 12; break;
            case "Heat #13": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Heat; round.Number = 13; break;
            case "Heat #14": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Heat; round.Number = 14; break;
            case "Heat #15": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Heat; round.Number = 15; break;
            case "Heat #16": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Heat; round.Number = 16; break;
            case "Heat #17": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Heat; round.Number = 17; break;
            case "Heat #2": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Heat; round.Number = 2; break;
            case "Heat #3": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Heat; round.Number = 3; break;
            case "Heat #4": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Heat; round.Number = 4; break;
            case "Heat #5": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Heat; round.Number = 5; break;
            case "Heat #6": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Heat; round.Number = 6; break;
            case "Heat #7": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Heat; round.Number = 7; break;
            case "Heat #8": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Heat; round.Number = 8; break;
            case "Heat #9": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Heat; round.Number = 9; break;
            case "Heat 1/2": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Heat; round.Info = "1-2"; break;
            case "Heat 1-6": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Heat; round.Info = "1-6"; break;
            case "Heat 3/4": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Heat; round.Info = "3-4"; break;
            case "Heat 5/6": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Heat; round.Info = "5-6"; break;
            case "Heat 5-8": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Heat; round.Info = "5-8"; break;
            case "Heat 7/8": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Heat; round.Info = "7-8"; break;
            case "Heat 7-12": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Heat; round.Info = "7-12"; break;
            case "Heat 9-12": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Heat; round.Info = "9-12"; break;
            case "Heat Eight": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Heat; round.Number = 8; break;
            case "Heat Eighteen": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Heat; round.Number = 18; break;
            case "Heat Eleven": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Heat; round.Number = 11; break;
            case "Heat Fifteen": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Heat; round.Number = 15; break;
            case "Heat Five": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Heat; round.Number = 5; break;
            case "Heat Four": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Heat; round.Number = 4; break;
            case "Heat Fourteen": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Heat; round.Number = 14; break;
            case "Heat Nine": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Heat; round.Number = 9; break;
            case "Heat One": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Heat; round.Number = 1; break;
            case "Heat One Re-Run": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Heat; round.Number = 1; round.Info = "Playoff"; break;
            case "Heat Seven": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Heat; round.Number = 7; break;
            case "Heat Seventeen": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Heat; round.Number = 17; break;
            case "Heat Six": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Heat; round.Number = 6; break;
            case "Heat Six Re-Run": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Heat; round.Number = 6; round.Info = "Playoff"; break;
            case "Heat Sixteen": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Heat; round.Number = 16; break;
            case "Heat Ten": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Heat; round.Number = 10; break;
            case "Heat Thirteen": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Heat; round.Number = 13; break;
            case "Heat Three": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Heat; round.Number = 3; break;
            case "Heat Three Re-run": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Heat; round.Number = 3; round.Info = "Playoff"; break;
            case "Heat Twelve": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Heat; round.Number = 12; break;
            case "Heat Two": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Heat; round.Number = 2; break;
            case "Heat Two Re-run": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Heat; round.Number = 2; round.Info = "Playoff"; break;
            case "Pool 1": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 1; break;
            case "Pool 1, Barrage": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 1; round.Info = "Barrage"; break;
            case "Pool 1, Barrage 2-5": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 1; round.Info = "Barrage"; break;
            case "Pool 1, Barrage 3-4": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 1; round.Info = "Barrage"; break;
            case "Pool 1, Barrage 3-5": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 1; round.Info = "Barrage"; break;
            case "Pool 1, Barrage 3-6": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 1; round.Info = "Barrage"; break;
            case "Pool 1, Barrage 4-5": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 1; round.Info = "Barrage"; break;
            case "Pool 1, Barrage 4-6": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 1; round.Info = "Barrage"; break;
            case "Pool 1, Barrage 6-8": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 1; round.Info = "Barrage"; break;
            case "Pool 10": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 10; break;
            case "Pool 10, Barrage 2-4": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 10; round.Info = "Barrage"; break;
            case "Pool 10, Barrage 3-4": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 10; round.Info = "Barrage"; break;
            case "Pool 11": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 11; break;
            case "Pool 11, Barrage 2-4": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 11; round.Info = "Barrage"; break;
            case "Pool 11, Barrage 3-5": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 11; round.Info = "Barrage"; break;
            case "Pool 12": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 12; break;
            case "Pool 12, Barrage 2-4": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 12; round.Info = "Barrage"; break;
            case "Pool 12, Barrage 3-4": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 12; round.Info = "Barrage"; break;
            case "Pool 13": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 13; break;
            case "Pool 14": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 14; break;
            case "Pool 15": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 15; break;
            case "Pool 16": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 16; break;
            case "Pool 17": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 17; break;
            case "Pool 2": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 2; break;
            case "Pool 2, Barrage 2-4": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 2; round.Info = "Barrage"; break;
            case "Pool 2, Barrage 3-4": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 2; round.Info = "Barrage"; break;
            case "Pool 2, Barrage 3-5": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 2; round.Info = "Barrage"; break;
            case "Pool 2, Barrage 3-7": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 2; round.Info = "Barrage"; break;
            case "Pool 2, Barrage 4-5": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 2; round.Info = "Barrage"; break;
            case "Pool 2, Barrage 4-6": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 2; round.Info = "Barrage"; break;
            case "Pool 2, Barrage 5-6": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 2; round.Info = "Barrage"; break;
            case "Pool 2, Barrage 5-8": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 2; round.Info = "Barrage"; break;
            case "Pool 2, Barrage 6-12": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 2; round.Info = "Barrage"; break;
            case "Pool 3": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 3; break;
            case "Pool 3, Barrage 3-5": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 3; round.Info = "Barrage"; break;
            case "Pool 3, Barrage 4-5": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 3; round.Info = "Barrage"; break;
            case "Pool 3, Barrage 4-6": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 3; round.Info = "Barrage"; break;
            case "Pool 3, Barrage 5-6": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 3; round.Info = "Barrage"; break;
            case "Pool 3, Barrage 6-8": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 3; round.Info = "Barrage"; break;
            case "Pool 4": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 4; break;
            case "Pool 4, Barrage": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 4; round.Info = "Barrage"; break;
            case "Pool 4, Barrage 2-4": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 4; round.Info = "Barrage"; break;
            case "Pool 4, Barrage 2-5": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 4; round.Info = "Barrage"; break;
            case "Pool 4, Barrage 3-4": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 4; round.Info = "Barrage"; break;
            case "Pool 4, Barrage 3-5": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 4; round.Info = "Barrage"; break;
            case "Pool 4, Barrage 4-5": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 4; round.Info = "Barrage"; break;
            case "Pool 4, Barrage 4-6": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 4; round.Info = "Barrage"; break;
            case "Pool 4, Barrage 6-8": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 4; round.Info = "Barrage"; break;
            case "Pool 5": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 5; break;
            case "Pool 5, Barrage 2-4": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 5; round.Info = "Barrage"; break;
            case "Pool 5, Barrage 3-4": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 5; round.Info = "Barrage"; break;
            case "Pool 5, Barrage 3-6": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 5; round.Info = "Barrage"; break;
            case "Pool 5, Barrage 4-6": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 5; round.Info = "Barrage"; break;
            case "Pool 5, Barrage 5-7": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 5; round.Info = "Barrage"; break;
            case "Pool 6": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 6; break;
            case "Pool 6, Barrage": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 6; round.Info = "Barrage"; break;
            case "Pool 6, Barrage 3-4": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 6; round.Info = "Barrage"; break;
            case "Pool 6, Barrage 3-5": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 6; round.Info = "Barrage"; break;
            case "Pool 6, Barrage 4-5": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 6; round.Info = "Barrage"; break;
            case "Pool 6, Barrage 5-6": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 6; round.Info = "Barrage"; break;
            case "Pool 7": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 7; break;
            case "Pool 7, Barrage 2-4": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 7; round.Info = "Barrage"; break;
            case "Pool 7, Barrage 3-5": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 7; round.Info = "Barrage"; break;
            case "Pool 7, Barrage 4-6": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 7; round.Info = "Barrage"; break;
            case "Pool 8": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 8; break;
            case "Pool 8, Barrage 2-4": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 8; round.Info = "Barrage"; break;
            case "Pool 8, Barrage 3-4": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 8; round.Info = "Barrage"; break;
            case "Pool 8, Barrage 3-5": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 8; round.Info = "Barrage"; break;
            case "Pool 8, Barrage 4-5": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 8; round.Info = "Barrage"; break;
            case "Pool 9": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 9; break;
            case "Pool A": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 1; break;
            case "Pool B": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 2; break;
            case "Pool C": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 3; break;
            case "Pool D": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 4; break;
            case "Pool E": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 5; break;
            case "Pool F": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 6; break;
            case "Pool Five": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 5; break;
            case "Pool Four": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 4; break;
            case "Pool G": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 7; break;
            case "Pool H": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 8; break;
            case "Pool One": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 1; break;
            case "Pool Three": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 3; break;
            case "Pool Two": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Pool; round.Number = 2; break;
            case "Round Five": round.Type = RoundTypeEnum.RoundFive; break;
            case "Round Four": round.Type = RoundTypeEnum.RoundFour; break;
            case "Round Four5": round.Type = RoundTypeEnum.RoundFour; break;
            case "Round One": round.Type = RoundTypeEnum.RoundOne; break;
            case "Round One Pool Five": round.Type = RoundTypeEnum.RoundOne; round.SubType = RoundTypeEnum.Pool; round.Number = 5; break;
            case "Round One Pool Four": round.Type = RoundTypeEnum.RoundOne; round.SubType = RoundTypeEnum.Pool; round.Number = 4; break;
            case "Round One Pool One": round.Type = RoundTypeEnum.RoundOne; round.SubType = RoundTypeEnum.Pool; round.Number = 1; break;
            case "Round One Pool Six": round.Type = RoundTypeEnum.RoundOne; round.SubType = RoundTypeEnum.Pool; round.Number = 6; break;
            case "Round One Pool Three": round.Type = RoundTypeEnum.RoundOne; round.SubType = RoundTypeEnum.Pool; round.Number = 3; break;
            case "Round One Pool Two": round.Type = RoundTypeEnum.RoundOne; round.SubType = RoundTypeEnum.Pool; round.Number = 2; break;
            case "Round One Repechage":
            case "Round One Repêchage": round.Type = RoundTypeEnum.RoundOne; round.SubType = RoundTypeEnum.Repechage; break;
            case "Round One Repechage Final": round.Type = RoundTypeEnum.RoundOne; round.SubType = RoundTypeEnum.Repechage; break;
            case "Round One Rerace": round.Type = RoundTypeEnum.RoundOne; round.SubType = RoundTypeEnum.Playoff; break;
            case "Round One, Heat Ten": round.Type = RoundTypeEnum.RoundOne; round.SubType = RoundTypeEnum.Heat; round.Number = 10; break;
            case "Round One1": round.Type = RoundTypeEnum.RoundOne; break;
            case "Round One9": round.Type = RoundTypeEnum.RoundOne; break;
            case "Round Seven": round.Type = RoundTypeEnum.RoundSeven; break;
            case "Round Six": round.Type = RoundTypeEnum.RoundSix; break;
            case "Round Three": round.Type = RoundTypeEnum.RoundThree; break;
            case "Round Three Repechage": round.Type = RoundTypeEnum.RoundThree; round.SubType = RoundTypeEnum.Repechage; break;
            case "Round Two": round.Type = RoundTypeEnum.RoundTwo; break;
            case "Round Two Repechage":
            case "Round Two Repêchage": round.Type = RoundTypeEnum.RoundTwo; round.SubType = RoundTypeEnum.Repechage; break;
            case "Round Two Repechage Final": round.Type = RoundTypeEnum.RoundTwo; round.SubType = RoundTypeEnum.Repechage; break;
            case "Round-Robin": round.Type = RoundTypeEnum.RoundRobin; break;
            case "Round Robin": round.Type = RoundTypeEnum.RoundRobin; break;
            case "Eighth-Finals": round.Type = RoundTypeEnum.Eightfinals; break;
            case "Elimination Round": round.Type = RoundTypeEnum.EliminationRound; break;
            case "Elimination Rounds, Round Five Repechage": round.Type = RoundTypeEnum.RoundFive; round.SubType = RoundTypeEnum.Repechage; break;
            case "Elimination Rounds, Round Five Repêchage": round.Type = RoundTypeEnum.RoundFive; round.SubType = RoundTypeEnum.Repechage; break;
            case "Elimination Rounds, Round Four": round.Type = RoundTypeEnum.RoundFour; break;
            case "Elimination Rounds, Round Four Repechage": round.Type = RoundTypeEnum.RoundFour; round.SubType = RoundTypeEnum.Repechage; break;
            case "Elimination Rounds, Round Four Repêchage": round.Type = RoundTypeEnum.RoundFour; round.SubType = RoundTypeEnum.Repechage; break;
            case "Elimination Rounds, Round One": round.Type = RoundTypeEnum.RoundOne; break;
            case "Elimination Rounds, Round One Repechage": round.Type = RoundTypeEnum.RoundOne; round.SubType = RoundTypeEnum.Repechage; break;
            case "Elimination Rounds, Round One Repêchage": round.Type = RoundTypeEnum.RoundOne; round.SubType = RoundTypeEnum.Repechage; break;
            case "Elimination Rounds, Round Three": round.Type = RoundTypeEnum.RoundThree; break;
            case "Elimination Rounds, Round Three Repechage": round.Type = RoundTypeEnum.RoundThree; round.SubType = RoundTypeEnum.Repechage; break;
            case "Elimination Rounds, Round Three Repêchage": round.Type = RoundTypeEnum.RoundThree; round.SubType = RoundTypeEnum.Repechage; break;
            case "Elimination Rounds, Round Two": round.Type = RoundTypeEnum.RoundTwo; break;
            case "Elimination Rounds, Round Two Repechage": round.Type = RoundTypeEnum.RoundTwo; round.SubType = RoundTypeEnum.Repechage; break;
            case "Elimination Rounds, Round Two Repêchage": round.Type = RoundTypeEnum.RoundTwo; round.SubType = RoundTypeEnum.Repechage; break;
            case "Compulsory Dance": round.Type = RoundTypeEnum.CompulsoryDance; break;
            case "Compulsory Dance 1": round.Type = RoundTypeEnum.CompulsoryDance; break;
            case "Compulsory Dance 2": round.Type = RoundTypeEnum.CompulsoryDance; break;
            case "Compulsory Dances": round.Type = RoundTypeEnum.CompulsoryDance; break;
            case "Compulsory Dances Summary": round.Type = RoundTypeEnum.CompulsoryDance; break;
            case "Compulsory Figures": round.Type = RoundTypeEnum.CompulsoryFigures; break;
            case "Figures": round.Type = RoundTypeEnum.CompulsoryFigures; break;
            case "Consolation Final": round.Type = RoundTypeEnum.ConsolationRound; round.SubType = RoundTypeEnum.Final; break;
            case "Consolation Round": round.Type = RoundTypeEnum.ConsolationRound; break;
            case "Consolation Round - Final": round.Type = RoundTypeEnum.ConsolationRound; round.SubType = RoundTypeEnum.Final; break;
            case "Consolation Round - Round One": round.Type = RoundTypeEnum.ConsolationRound; round.SubType = RoundTypeEnum.RoundOne; break;
            case "Consolation Round - Semi-Finals": round.Type = RoundTypeEnum.ConsolationRound; round.SubType = RoundTypeEnum.Semifinals; break;
            case "Consolation Round: Final": round.Type = RoundTypeEnum.ConsolationRound; round.SubType = RoundTypeEnum.Final; break;
            case "Consolation Round: Quarter-Finals": round.Type = RoundTypeEnum.ConsolationRound; round.SubType = RoundTypeEnum.Quarterfinals; break;
            case "Consolation Round: Semi-Finals": round.Type = RoundTypeEnum.ConsolationRound; round.SubType = RoundTypeEnum.Semifinals; break;
            case "Consolation Tournament": round.Type = RoundTypeEnum.ConsolationRound; break;
            case "Qualification": round.Type = RoundTypeEnum.Qualification; break;
            case "Qualification Round": round.Type = RoundTypeEnum.Qualification; break;
            case "Qualifying": round.Type = RoundTypeEnum.Qualification; break;
            case "Qualifying Round": round.Type = RoundTypeEnum.Qualification; break;
            case "Qualifying Round 1": round.Type = RoundTypeEnum.RoundOne; break;
            case "Qualifying Round 2": round.Type = RoundTypeEnum.RoundTwo; break;
            case "Qualifying Round One": round.Type = RoundTypeEnum.RoundOne; break;
            case "Qualifying Round Two": round.Type = RoundTypeEnum.RoundTwo; break;
            case "Qualifying Round, Group A": round.Type = RoundTypeEnum.Qualification; round.SubType = RoundTypeEnum.Group; round.Number = 1; break;
            case "Qualifying Round, Group A Re-Jump": round.Type = RoundTypeEnum.Qualification; round.SubType = RoundTypeEnum.Group; round.Number = 1; round.Info = "Playoff"; break;
            case "Qualifying Round, Group A1": round.Type = RoundTypeEnum.Qualification; round.SubType = RoundTypeEnum.Group; round.Number = 1; break;
            case "Qualifying Round, Group B": round.Type = RoundTypeEnum.Qualification; round.SubType = RoundTypeEnum.Group; round.Number = 2; break;
            case "Qualifying Round, Group B1": round.Type = RoundTypeEnum.Qualification; round.SubType = RoundTypeEnum.Group; round.Number = 2; break;
            case "Qualifying Round, Group C": round.Type = RoundTypeEnum.Qualification; round.SubType = RoundTypeEnum.Group; round.Number = 3; break;
            case "Qualifying Round, Group C3": round.Type = RoundTypeEnum.Qualification; round.SubType = RoundTypeEnum.Group; round.Number = 3; break;
            case "Qualifying Round, Group D": round.Type = RoundTypeEnum.Qualification; round.SubType = RoundTypeEnum.Group; round.Number = 4; break;
            case "Qualifying Round, Group D4": round.Type = RoundTypeEnum.Qualification; round.SubType = RoundTypeEnum.Group; round.Number = 4; break;
            case "Qualifying Round, Group E": round.Type = RoundTypeEnum.Qualification; round.SubType = RoundTypeEnum.Group; round.Number = 5; break;
            case "Qualifying Round, Group F": round.Type = RoundTypeEnum.Qualification; round.SubType = RoundTypeEnum.Group; round.Number = 6; break;
            case "Qualifying Round, Group One": round.Type = RoundTypeEnum.Qualification; round.SubType = RoundTypeEnum.Group; round.Number = 1; break;
            case "Qualifying Round, Group Two": round.Type = RoundTypeEnum.Qualification; round.SubType = RoundTypeEnum.Group; round.Number = 2; break;
            case "Classification Final 1": round.Type = RoundTypeEnum.Classification; round.SubType = RoundTypeEnum.Final; break;
            case "Classification Final 2": round.Type = RoundTypeEnum.Classification; round.SubType = RoundTypeEnum.Final; break;
            case "Classification Round Five": round.Type = RoundTypeEnum.Classification; round.SubType = RoundTypeEnum.RoundFive; break;
            case "Classification Round Four": round.Type = RoundTypeEnum.Classification; round.SubType = RoundTypeEnum.RoundFour; break;
            case "Classification Round One": round.Type = RoundTypeEnum.Classification; round.SubType = RoundTypeEnum.RoundOne; break;
            case "Classification Round Six": round.Type = RoundTypeEnum.Classification; round.SubType = RoundTypeEnum.RoundSix; break;
            case "Classification Round Three": round.Type = RoundTypeEnum.Classification; round.SubType = RoundTypeEnum.RoundThree; break;
            case "Classification Round Two": round.Type = RoundTypeEnum.Classification; round.SubType = RoundTypeEnum.RoundTwo; break;
            case "Race Eight": round.Type = RoundTypeEnum.RaceEight; break;
            case "Race Five": round.Type = RoundTypeEnum.RaceFive; break;
            case "Race Four": round.Type = RoundTypeEnum.RaceFour; break;
            case "Race Nine": round.Type = RoundTypeEnum.RaceNine; break;
            case "Race One": round.Type = RoundTypeEnum.RaceOne; break;
            case "Race Seven": round.Type = RoundTypeEnum.RaceSeven; break;
            case "Race Six": round.Type = RoundTypeEnum.RaceSix; break;
            case "Race Ten": round.Type = RoundTypeEnum.RaceTen; break;
            case "Race Three": round.Type = RoundTypeEnum.RaceThree; break;
            case "Race Two": round.Type = RoundTypeEnum.RaceTwo; break;
            case "Ranking Round": round.Type = RoundTypeEnum.RankingRound; break;
            case "Lucky Loser Round": round.Type = RoundTypeEnum.LuckyLoserRound; break;
            case "Jump-Off": round.Type = RoundTypeEnum.Playoff; break;
            case "Jump-Off for 1-2": round.Type = RoundTypeEnum.Playoff; round.Info = "1-2"; break;
            case "Jump-off for 2-4": round.Type = RoundTypeEnum.Playoff; round.Info = "2-4"; break;
            case "Jump-Off for 3-4": round.Type = RoundTypeEnum.Playoff; round.Info = "3-4"; break;
            case "Jump-off for 3-5": round.Type = RoundTypeEnum.Playoff; round.Info = "3-5"; break;
            case "Jump-Off for 3-9": round.Type = RoundTypeEnum.Playoff; round.Info = "3-9"; break;
            case "Jump-off for 6-7": round.Type = RoundTypeEnum.Playoff; round.Info = "6-7"; break;
            case "Seeding Round": round.Type = RoundTypeEnum.RankingRound; break;
            case "Shoot-Off": round.Type = RoundTypeEnum.Playoff; break;
            case "Shoot-Off 1": round.Type = RoundTypeEnum.Playoff; break;
            case "Shoot-Off 2": round.Type = RoundTypeEnum.Playoff; break;
            case "Shoot-Off for 1st Place": round.Type = RoundTypeEnum.Playoff; round.SubType = RoundTypeEnum.GoldMedalMatch; break;
            case "Shoot-Off for 2nd Place": round.Type = RoundTypeEnum.Playoff; round.SubType = RoundTypeEnum.SilverMedalMatch; break;
            case "Shoot-Off for 3rd Place": round.Type = RoundTypeEnum.Playoff; round.SubType = RoundTypeEnum.BronzeMedalMatch; break;
            case "Short Dance": round.Type = RoundTypeEnum.ShortProgram; break;
            case "Short Program": round.Type = RoundTypeEnum.ShortProgram; break;
            case "Swim-Off": round.Type = RoundTypeEnum.Playoff; break;
            case "Swim-Off for 16th Place": round.Type = RoundTypeEnum.Playoff; round.Info = "16-17"; break;
            case "Swim-Off for 16th Place - Race 1": round.Type = RoundTypeEnum.Playoff; round.Info = "16-17"; break;
            case "Swim-Off for 16th Place - Race 2": round.Type = RoundTypeEnum.Playoff; round.Info = "16-17"; break;
            case "Swim-Off for 8th Place": round.Type = RoundTypeEnum.Playoff; round.Info = "8-9"; break;
            case "Swim-Off for Places 7-8": round.Type = RoundTypeEnum.Playoff; round.Info = "7-8"; break;
            case "Third-Place Tournament": round.Type = RoundTypeEnum.Repechage; round.SubType = RoundTypeEnum.BronzeMedalMatch; break;
            case "Tie-Breaker": round.Type = RoundTypeEnum.Playoff; break;
            case "Second Place Tournament - Final": round.Type = RoundTypeEnum.Repechage; round.SubType = RoundTypeEnum.SilverMedalMatch; break;
            case "Second Place Tournament - Round One": round.Type = RoundTypeEnum.Repechage; round.SubType = RoundTypeEnum.RoundOne; break;
            case "Second Place Tournament - Round Two": round.Type = RoundTypeEnum.Repechage; round.SubType = RoundTypeEnum.RoundTwo; break;
            case "Second Place Tournament - Semi-Finals": round.Type = RoundTypeEnum.Repechage; round.SubType = RoundTypeEnum.Semifinals; break;
            case "Second-Place Tournament": round.Type = RoundTypeEnum.Repechage; round.SubType = RoundTypeEnum.SilverMedalMatch; break;
            case "Second-to-Fifth Place Tournament": round.Type = RoundTypeEnum.Repechage; round.SubType = RoundTypeEnum.SilverMedalMatch; break;
            case "Match 1/2": round.Type = RoundTypeEnum.GoldMedalMatch; break;
            case "Match 1-6": round.Type = RoundTypeEnum.GoldMedalMatch; break;
            case "Match 3/4": round.Type = RoundTypeEnum.BronzeMedalMatch; break;
            case "Match 5-7": round.Type = RoundTypeEnum.Classification; round.Info = "5-7"; break;
            case "Match 5-8": round.Type = RoundTypeEnum.Classification; round.Info = "5-8"; break;
            case "Match 7-10": round.Type = RoundTypeEnum.Classification; round.Info = "7-10"; break;
            case "Match 9-12": round.Type = RoundTypeEnum.Classification; round.Info = "9-12"; break;
            case "Grand Prix": round.Type = RoundTypeEnum.GrandPrix; break;
            case "Grand Prix Freestyle": round.Type = RoundTypeEnum.GrandPrix; break;
            case "Grand Prix Special": round.Type = RoundTypeEnum.GrandPrix; break;
            case "Free Dance": round.Type = RoundTypeEnum.FreeSkating; break;
            case "Free Skating": round.Type = RoundTypeEnum.FreeSkating; break;
            case "Play-Off for Bronze Medal": round.Type = RoundTypeEnum.BronzeMedalMatch; break;
            case "Play-Off for Silver Medal": round.Type = RoundTypeEnum.SilverMedalMatch; break;
            case "Play-offs": round.Type = RoundTypeEnum.Playoff; break;
            case "Repechage": round.Type = RoundTypeEnum.Repechage; break;
            case "Repêchage": round.Type = RoundTypeEnum.Repechage; break;
            case "Repêchage Final": round.Type = RoundTypeEnum.Repechage; round.SubType = RoundTypeEnum.Final; break;
            case "Repechage Final": round.Type = RoundTypeEnum.Repechage; round.SubType = RoundTypeEnum.Final; break;
            case "Repechage Heats": round.Type = RoundTypeEnum.Repechage; break;
            case "Repechage Round One": round.Type = RoundTypeEnum.Repechage; round.SubType = RoundTypeEnum.RoundOne; break;
            case "Repechage Round Two": round.Type = RoundTypeEnum.Repechage; round.SubType = RoundTypeEnum.RoundTwo; break;
            case "Rhythm Dance": round.Type = RoundTypeEnum.RhythmDance; break;
            case "A Final": round.Type = RoundTypeEnum.Final; round.Info = "A"; break;
            case "B Final": round.Type = RoundTypeEnum.Final; round.Info = "B"; break;
            case "Final": round.Type = RoundTypeEnum.Final; break;
            case "Final A": round.Type = RoundTypeEnum.Final; round.Info = "A"; break;
            case "Final B": round.Type = RoundTypeEnum.Final; round.Info = "B"; break;
            case "Final C": round.Type = RoundTypeEnum.Final; round.Info = "C"; break;
            case "Final D": round.Type = RoundTypeEnum.Final; round.Info = "D"; break;
            case "Final E": round.Type = RoundTypeEnum.Final; round.Info = "E"; break;
            case "Final F": round.Type = RoundTypeEnum.Final; round.Info = "F"; break;
            case "Final Heat": round.Type = RoundTypeEnum.Repechage; break;
            case "Final Heat One": round.Type = RoundTypeEnum.Repechage; break;
            case "Final Heat Two": round.Type = RoundTypeEnum.Repechage; break;
            case "Final Pool": round.Type = RoundTypeEnum.FinalRound; break;
            case "Final Pool Barrage 2-3": round.Type = RoundTypeEnum.SilverMedalMatch; break;
            case "Final Pool, Barrage #1 1-2": round.Type = RoundTypeEnum.GoldMedalMatch; break;
            case "Final Pool, Barrage #2 1-2": round.Type = RoundTypeEnum.GoldMedalMatch; break;
            case "Final Pool, Barrage 1-2": round.Type = RoundTypeEnum.GoldMedalMatch; break;
            case "Final Pool, Barrage 1-3": round.Type = RoundTypeEnum.FinalRound; break;
            case "Final Pool, Barrage 1-4": round.Type = RoundTypeEnum.FinalRound; break;
            case "Final Pool, Barrage 2-3": round.Type = RoundTypeEnum.FinalRound; break;
            case "Final Pool, Barrage 2-4": round.Type = RoundTypeEnum.FinalRound; break;
            case "Final Pool, Barrage 2-5": round.Type = RoundTypeEnum.FinalRound; break;
            case "Final Pool, Barrage 3-4": round.Type = RoundTypeEnum.BronzeMedalMatch; break;
            case "Final Pool, Barrage 3-5": round.Type = RoundTypeEnum.BronzeMedalMatch; break;
            case "Final Pool, Barrage 4-5": round.Type = RoundTypeEnum.Classification; round.Info = "4-5"; break;
            case "Final Pool, Barrage 6-7": round.Type = RoundTypeEnum.Classification; round.Info = "6-7"; break;
            case "Final Round": round.Type = RoundTypeEnum.FinalRound; break;
            case "Final Round 1": round.Type = RoundTypeEnum.FinalRound; break;
            case "Final Round 2": round.Type = RoundTypeEnum.FinalRound; break;
            case "Final Round 3": round.Type = RoundTypeEnum.FinalRound; break;
            case "Final Round One": round.Type = RoundTypeEnum.FinalRound; break;
            case "Final Round Three": round.Type = RoundTypeEnum.FinalRound; break;
            case "Final Round Two": round.Type = RoundTypeEnum.FinalRound; break;
            case "Final Round2": round.Type = RoundTypeEnum.FinalRound; break;
            case "Final, Swim-Off": round.Type = RoundTypeEnum.Final; round.SubType = RoundTypeEnum.Playoff; break;
            case "Final1": round.Type = RoundTypeEnum.Final; break;
            case "First Final": round.Type = RoundTypeEnum.Final; break;
            case "Fleet Races": round.Type = RoundTypeEnum.FleetRaces; break;
            case "Medal Pool": round.Type = RoundTypeEnum.RoundTwo; break;
            case "Original Final": round.Type = RoundTypeEnum.Final; break;
            case "Original Round One": round.Type = RoundTypeEnum.RoundOne; break;
            case "Original Set Pattern Dance": round.Type = RoundTypeEnum.OriginalSetPatternDance; break;
            case "Re-run Final": round.Type = RoundTypeEnum.Playoff; break;
            case "Re-run of Heat Two": round.Type = RoundTypeEnum.PreliminaryRound; round.SubType = RoundTypeEnum.Heat; round.Number = 2; round.Info = "Playoff"; break;
            case "Free Routine": round.Type = RoundTypeEnum.FreeRoutine; break;
            case "Technical Routine": round.Type = RoundTypeEnum.TechnicalRoutine; break;
        }

        return round;
    }

    public RoundTypeEnum MapAdditionalRound(string name)
    {
        var round = RoundTypeEnum.None;

        switch (name)
        {
            case "Downhill":
            case "Downhill1":
                round = RoundTypeEnum.Downhill;
                break;
            case "Run #1":
            case "Run #11":
                round = RoundTypeEnum.Run1;
                break;
            case "Run #2":
            case "Run #21":
                round = RoundTypeEnum.Run2;
                break;
            case "Slalom":
            case "Slalom1":
                round = RoundTypeEnum.Slalom;
                break;
        }

        return round;
    }
}