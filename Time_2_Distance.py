import math
import numpy as np
import pandas as pd

# This converts data in the time domain to data in the distance domain
# Be careful of the required inputs as they are quite specific
# It is set-up to deal with the output from Data_Collator

def OverSample(Data,multiplication): # This function interpolates and creates more data-points for time/speed to distance/speed calculations
    # This increases the steps such that each interval is less than a meter
    # The oversampling is a little overkill as the multiplication factor is worked out in kmph and then m/s is used.
    new_frame = pd.DataFrame() # Hollow output frame
    length = Data.shape[0] # Extracting length before appending values
    for x in range(0, Data.shape[1]):  # Across all columns of the data
        count = -1 # Counter variable starts at -1 as first iteration will increase it by 1
        temp_matrix = [0] *(Data.shape[0] * multiplication)  # Creating a working matrix of the right size
        speed_matrix = Data.loc[:,'run'+str(x+1)].values # Extracting the speed data from that run
        for i in range(0,((Data.shape[0] * multiplication-multiplication))): # Along the whole data-length
            if (i / multiplication) % 1 == 0:
                count += 1
                duplicate = 1
            if i == 0:
                temp_matrix[i] = 0
            else:
                if duplicate == 1:
                    temp_matrix[i] = speed_matrix[count]
                    duplicate = 0
                else:
                    temp_matrix[i] = np.interp(i/multiplication,[math.floor(i/multiplication),math.ceil(i/multiplication)],[speed_matrix[count],speed_matrix[count+1]])
        new_frame[('run' + str(x + 1))] = temp_matrix
    return new_frame

def Speed_at_Distance(data,multiplication): # This function takes in a DataFrame of speeds at time and converts it to speed at distance
    # Data must be a pandas DataFrame
    Data1 = data*0.277777778 # Converting to m/s
    Data = OverSample(Data1,multiplication) # OverSampling by a factor to make the steps small enough
    distances = pd.Series.cumsum(Data*(0.277777778/multiplication)) # Converting to m/s
    Max_Dist = math.ceil(max(distances.select_dtypes(include=[np.number]).max())) # Length of the new dataframe
    new_frame = pd.DataFrame()
    for x in range(0,Data.shape[1]): # Across all columns of the data
        speed_matrix = Data.loc[:,'run'+str(x+1)].values # Extracting speed
        dist_mat = distances.loc[:,'run'+str(x+1)].values  # Cumulative distance as a matrix
        temp_matrix = [0]*Max_Dist # Creating a working matrix
        count1 = 1 # Counting initialisation for distance incriments
        for i in range(0,distances.shape[0]): # Through all values
            if dist_mat[i] > count1:
                speed = np.interp(count1,[dist_mat[i-1],dist_mat[i]],[speed_matrix[i-1],speed_matrix[i]])
                temp_matrix[count1] = speed
                count1 += 1
        new_frame[('run' + str(x + 1))] = np.append(temp_matrix,[0])
    new_frame /= 0.277777778
    return new_frame

def Shell(Data,Hz): # This function handles the conversion so it can easily be externally used
    # The input should be a a pandas DataFrame with run1 to runn
    gap_test = pd.Series.cumsum(Data,0)*0.277777778/Hz # This converts to m/s
    largest_gap = 0 # A variable to test the maximum gap in the data
    for x in range(0,gap_test.shape[1]):
        for i in range(1,gap_test.shape[0]):
            gap = abs(gap_test.iloc[i,x] - gap_test.iloc[i-1,x]) # The difference between the previous and current value
            if gap > largest_gap: # Testing if this gap is the biggest yet
                largest_gap = gap # If a new biggest gap is found then it is logged
    multiplication = int(math.ceil(largest_gap)) # The factor in km/h
    Output = Speed_at_Distance(Data,multiplication)
    # Output is a DataFrame of the same data with distance as the base
    return Output


