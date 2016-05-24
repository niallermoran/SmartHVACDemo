
  WITH subquery AS (  
        SELECT *, ComfortLevelPrediction ( FloorArea, NumberofPeople ) as result 
		from IoTHub TIMESTAMP by Time  
    )  

/* Return values where the internal temp is less than comfort level and heating is off */	
Select * ,result.[ComfortLevelTemp], 'TurnOn' as Action
    Into TurnOn  
    From subquery
    where Cast( InternalTemp as FLOAT) < cast( result.[ComfortLevelTemp] as FLOAT) and IsHeatingOn = 0
	
/* Return values where the internal temp is greater than comfort level + 2 and heating is on */	
Select * ,result.[ComfortLevelTemp], 'TurnOff' as Action
    Into TurnOff  
    From subquery
    where cast( InternalTemp as float) > cast( result.[ComfortLevelTemp] as float) + 2 and IsHeatingOn = 1	
    
 	