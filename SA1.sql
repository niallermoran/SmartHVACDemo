/* we don't really want to push the entire stream out, this is just run for a while to get some Date for training our predictive model */
		
select *
INTO
	Streaming
from IoTHub TIMESTAMP by Time 

	
/* The tumbling window below will take an average every 15 minutes and output one record every 15 minutes to the Table 'Tumbling' */        
SELECT
    DeviceId,
	DeviceName,
	FloorArea,
	Avg(ExternalTemp) as ExternalTemp,
	Max(Time) as Time,
    Avg(Internaltemp) as InternalTemp,
    sum(IsHeatingOn) as IsHeatingOnTotal,
	count(*) as TotalReadings,
	sum(IsHeatingOn)*100/count(*) as HeatOnPercentage 
    Into Tumbling
FROM 
    IoTHub TIMESTAMP BY Time
GROUP BY 
    TumblingWindow(minute, 15), DeviceId, DeviceName, FloorArea

	

/* The sliding window below will output to the Table 'sliding' for every data point with averages for 15 minutes before, this is a rolling average every second.
We use this sliding window to check for alert conditions, e.g. average temperature over any 15 minutes should not go below 18 or over 23 
*/     

   
SELECT
    DeviceId,
	DeviceName,
	FloorArea,
	Avg(ExternalTemp) as ExternalTemp,
	Max(Time) as Time,
    Avg(Internaltemp) as InternalTemp,
    sum(IsHeatingOn) as IsHeatingOnTotal,
	count(*) as TotalReadings,
	sum(IsHeatingOn)*100/count(*) as HeatOnPercentage 
    Into Sliding
FROM 
    IoTHub TIMESTAMP BY Time
GROUP BY 
    SlidingWindow(minute, 15), DeviceId, DeviceName, FloorArea
/* Having Avg(InternalTemp) <= 18 or avg(InternalTemp) >= 23*/
 
        
 

