# This first function just holds the methematical logic for the windows and returns it in a Pandas DataFrame
def Window_Define(Data,Width,Overlap): # This function defines the bounds of the windows to allow for that information to be easily accessed later
    # This code steals the logic from the main windowing script and puts the limits into a DataFrame allowing windows to be referenced easily
    Definitions = pd.DataFrame() # Creating the empty DataFrame
    if 2*Overlap > Width: # Stopping overlap being unrealistic
        Overlap = math.floor(Width/2) # Defining limit
    Windows = math.floor((Data.shape[0]-Overlap)/(Width-Overlap)) # Max number of windows
    for i in range(1, Windows + 1):  # Through the length of all windows
        Definitions.loc[i-1,'Min_Point'] = Width * (i - 1) - Overlap * (i - 1)  # First in the window
        Definitions.loc[i-1,'Max_Point'] = Width * i - Overlap * (i - 1) - 1  # Last point in the window
    return Definitions

# This second function had the ability to define windows in line with the AR Yule functionality.
# This could easily be edited to perform another function as the core requirements are there
def Window_Cluster_Mode(Data,Width,Overlap,Mode_Definitions,Mode_Matrix): # A personalised windowing function
    # I felt that the pandas windowing options weren't quite what I wanted so I made this instead.
    # This cuts the data into some specific window sizes that look more like the diagram below.
    #[________]      [________]      [________]            Where the length is Width and the gap is
    #        [________]      [________]      [________]     given by (Width - 2 * Overlap)
    # An accepted list of actions are: Mean , SDev , Max , Min    and can be more than 1. Default is Mean
    # As ever Data must be a pandas DataFrame.

    # We need to work out how many steps we need to do.
    if 2*Overlap > Width: # Stopping overlap being unrealistic.
        Overlap = math.floor(Width/2)
    Windows = math.floor((Data.shape[0]-Overlap)/(Width-Overlap)) # Max number of windows
    # Then we initialise lots of DataFrames
    Columns = ['run'+ str(x+1) for x in range(0,Data.shape[1])] # Initialising the columns variable
    # The indexing is the first index of the original data starting at 0
    Index = [Width*(x-1)-Overlap*(x-1) for x in range(1,Windows+1)] # Index is the first point in each window
    Mode_Data = pd.DataFrame(index = Index, columns = Columns)  # Hollow output frame initialised

    # Then we cycle through all runs
    for runs in range(0,Data.shape[1]): # Across the width of the data.
        for i in range(1,Windows+1): # Through the length of all windows
            Min_Point = Width*(i-1)-Overlap*(i-1) # First in the window
            Max_Point = Width*i - Overlap*(i-1) - 1 # Last point in the window
            Test = Data.loc[Min_Point:Max_Point,'run' + str(runs+1)].values # Extracting the window of data
            # It is at this point that we have all of the data we need and can perform any function we want
            m = sum(Test)/Width # This is the mean
            for j in range(1,Mode_Definitions.shape[1]+1): # Running 1 upwards for simplicity later in the loop through all modes
                if Mode_Definitions.loc['Lower_Speed','Mode'+str(j)] > m: # Testing when the modes become larger than the mean of current window
                    Mode_band = j-3 # This refers us to the last band in which the mean fitted
                    break
                else:
                    Mode_band = Mode_Definitions.shape[1]-2 # The max band
            Sdev = stat.stdev(Test) # Standard deviation of data
            count = 0
            Dif = [0]*(len(Test)) # A variable that demonstrates if something is generally rising or falling
            for k in Test:
                if not count:
                    last = k
                    count += 1
                else:
                    if (k - last) > 0:
                        Dif[count] = 1 # Denoting that the value has increased
                    elif (k - last) < 0:
                        Dif[count] = -1 # Denoting that the value has decreased
                    else:
                        Dif[count] = 0 # Then it is the same (this will only really happen with 0's)
                    count += 1
                    last = k
            gap = Test[0] - Test[-1] # Taking the difference between the first and the last values to get an idea of the difference across the whole window
            # A positive value for gap means that there has been deceleration overall and negative indicates overall acceleration.
            if sum(Dif)/(Width-1) >= 0.25 and gap < 0:
                Mode_band += 0
            elif sum(Dif)/(Width-1) <= -0.25 and gap > 0:
                Mode_band += 1
            else:
                Mode_band += 2

            Mode_Data.loc[Min_Point,'run'+str(runs+1)] = Mode_band # Setting the output to the
            Mode_Matrix.loc['Counted','Mode'+str(Mode_band)] += Width # Counting how many of each mode we see

    return Mode_Data , Mode_Matrix
