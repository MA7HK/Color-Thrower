using UnityEngine;

public class Currency
{
	public static string DisplayCurrency(int gold) {
		int goldLength = gold.ToString().Length;
		char firstChar = gold.ToString()[0];
		char secondChar = gold.ToString()[1];
		char thirdChar = gold.ToString()[2];

		//	1000 -> 1k
		//	15,0000 -> 15k
		switch (goldLength)
		{	
			case 1 :
			case 2 :
			case 3 :
				return gold.ToString();

			//	1k - 9k
			case 4 : 
				return $"{firstChar}K";
				
			//	10k - 90k
			case 5 : 
				return $"{firstChar}{secondChar}K";

			//	100k - 999k
			case 6 :
				return $"{firstChar}{secondChar}{thirdChar}K";
			
			//	1.00M - 9.99M
			case 7 :
				return $"{firstChar}.{secondChar}{thirdChar}M";
			
			//	10.0M - 99.9M
			case 8 :
				if (gold >= 10000000 && gold <= 99999999) {
					return $"{firstChar}{secondChar}.{thirdChar}M";
				}
				break;

			//	100M - 999M
			case 9 :
				if (gold >= 100000000 && gold <= 999999999) {
					return $"{firstChar}{secondChar}{thirdChar}M";
				}
				break;
			
			// 1.00B - 9.99B
			case 10:
				if (gold >= 1000000000 && gold <= 9999999999) {
					return $"{firstChar}{secondChar}{thirdChar}B";
				}
    		break;
		}
		return string.Empty;
	}
}