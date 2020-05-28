# This generates random but realistic drive cycles

# First import all the required packages
import math
import numpy as np
import pandas as pd

# At the moment we don't have any actual data so we use the data generator.
# Not sure if there is a better way to run it so I'll ust copy the code into here.
# The following code labelled as section 1 is taken directly from Top Generator.py:
# ----------------------------------------    Section 1 - Generator   -----------------------------------------
# The previously made generator is run here in a function.
def TraceGen(SetTrans,length): # Allows the generator to be easily run
    # Defining how we want the drive cycle to run
    include = [0, 1, 2, 3]  # Defines driving modes allowed. All modes would be [0,1,2,3]
    Speeds = [[0, 50],  # Town speeds - [Min, Max]
              [20, 80],  # B_Road speeds - [Min, Max]
              [40, 110],  # A_Road speeds - [Min, Max]
              [80, 130]]  # Highway speeds - [Min, Max]
    maxrate = 6  # Max change of speed (kmph/s)

    # Defining forced transition times
    # Structure is [mode to transition to , time]
    # This line used to define the set-trans point but now this is defined in a function.

    # Defining different regimes
    def Drive(v, x, dclen, mode):  # For a given length, speed and time position
        input = np.random.randint(-600, 600) / 100  # This is a random change in speed
        v += input  # Apply this random change to the current speed.
        if v < Speeds[mode][0]:  # Detecting if modes have been exceeded
            v = Speeds[mode][0]  # Limiting to the minimum of that mode
            if mode > 0:  # If mode isn't the lowest then we decrease the mode by 1.
                if mode - 1 in include:
                    mode -= 1
                elif mode - 2 in include:
                    mode -= 2
                elif mode - 3 in include:
                    mode -= 3
        if v > Speeds[mode][1]:
            v = Speeds[mode][1]
            if mode < 3:
                if mode + 1 in include:
                    mode += 1
                elif mode + 2 in include:
                    mode += 2
                elif mode + 3 in include:
                    mode += 3
        return v, mode


    def Transition(v, x, dclen, mode, postmode):
        timer = 0
        rate = 0
        if mode == postmode:
            return mode, timer, rate
        if v < Speeds[postmode][0]:
            dif = v - Speeds[postmode][0]
            rate = maxrate
        elif v > Speeds[postmode][1]:
            dif = Speeds[postmode][1] - v
            rate = maxrate * -1
        else:
            dif = v - Speeds[postmode][0]
            rate = maxrate * -1
        timer = x + abs(math.ceil(dif / maxrate))
        mode = postmode
        return mode, timer, rate

    dclen = length  # Setting the drive cycle length
    SetTrans.append([0, dclen + 3])  # To stop the if statement going out of range we append an unreachable transition
    #print(SetTrans) I forget why this was here so I've commented it out
    mode = min(include) # Starting mode is the smallest allowed
    v = 0  # Starting speed = 0
    vvect = [0] * (dclen + 1) # Initialise the main vector
    modetrack = [0] * (dclen + 1) # Initialise a vector to allow visualisation of the mode
    tick = 0  # Counter hold speed at 0 for 10 seconds
    sit = 0  # Binary for stationary
    go = 0  # Binary for moving
    Transitioncount = 0  # Checks for how many transaitions have occured
    timer = 0  # Transition variable needs to be set because of if statement
    for x in range(1, dclen + 1, 1):  # Running at steps of a second
        if SetTrans != [0]:  # If a transition is defined
            if x == SetTrans[Transitioncount][1]:  # Have we reached the next transition
                mode, timer, rate = Transition(v, x, dclen, mode, SetTrans[Transitioncount][0]) # If so perform a transition
                Transitioncount += 1 # Record that we performed a transition
        if x <= timer: # Are we still in a transition
            v += rate # If so perform transition operation
        elif v == 0 and tick == 0 and sit == 0:  # Stationary for 9 seconds at the start and whenever stopped
            go = 0  # Start counter
            sit = 1 # Announce sit variable
        elif go == 1: # If we can go.....
            v, mode = Drive(v, x, dclen, mode)  # The actual generator is run
            if x > dclen - 70: # Are we near the end
                v = vvect[x - 1] - v / (dclen + 2 - x)
            if tick > 0: # Checking the tick counter is greater than 0
                tick -= 1 # Reducing the tick counter
            elif tick == 0: # If tick has become 0
                sit = 0 # Set sit to 0, we can now set off.
        else:
            tick += 1 # This allows us to have a delay before we have to sit again.
        if tick == 9: # Once we have restored tick to 9.....
            go = 1 # We can go again
        modetrack[x] = mode * 43 # This is purely to make mode match to speed

        vvect[x] = v # Append and track the speed calculated above
    smoothed = round(pd.Series(vvect).rolling(window=6).mean(),3) # This ust smoothes the trace slightly
    detect = np.count_nonzero(np.isnan(smoothed)) # There are NaN values at the start of the array and I'm not sure why
    smoothed = np.concatenate((smoothed[detect:dclen+1],np.transpose([0]*detect))) # Remove these values and add as many 0's to the end
    # Very occasionally negative values are returned so they are set to 0 below:
    smoothed[smoothed<0] = 0 # All points below 0 are set to 0.
    return smoothed # Return the final vector

# We now want to use our created function to generate a set of data on demand
def DataGen(number,length): # Request an amount of traces
    # We will use a pandas dataframe for this purpose that is defined below:
    label = 'run'
    for x in range(0,number): # Cycle through however many times is requested
        smoothed = TraceGen([[0,87],[0,90],[0,95],[0,100],[0,105],[0,110],[0,115],[0,120],[0,125],[0,130],[0,135],[1,140],[0,1520]],length) # A new random cycle each time  ///// [[3,400],[0,1400]] // [[3,200],[3,500],[3,900],[3,1100],[3,1400]]  //////[[1,100],[1,150],[1,200],[1,250],[1,300],[1,350],[3,400],[3,450],[3,500],[3,550],[3,600],[3,650],[3,700],[3,750],[3,800],[3,850],[3,900],[3,950],[3,1000],[1,1050],[1,1100],[1,1150],[1,1200],[1,1250],[1,1300],[1,1350]]
        if x == 0: # If we're on our first iteration
            Data = pd.DataFrame({label+str(x+1):smoothed}) # Creating the dataframe
        else:
            Data[label+str(x+1)] = smoothed # Appending new trace and labelling it accordingly
    return Data
# At this point we have a Pandas DataFrame containing all our runs labelled from "run1" to "runx"

# This is a output-test line:
#Data = DataGen(20,1800)

# We will also use the Data_Collator module to do Accelerator possition derivations

def AccelPos(SpeedTrace,Mass,outputs): # This just outputs accelerator pedal possition as a percentage
    # Checking if the input is a pandas DataFrame of a vector
    BaseVehiclePower = 100000 # Set to 100,000 Watts as a midrange value
    if isinstance(SpeedTrace, pd.DataFrame):
        Runs = len(SpeedTrace.columns)
        DataFrame = 1
    elif isinstance(SpeedTrace,numpy.array()):
        Runs = SpeedTrace.shape[1] # If this is a numpy array you will recieve the width of it
        DataFrame = 0
    elif SpeedTrace.shape[1] == 1:
        Runs = 1
        DataFrame = 0
    else: # When the input is unexpected
        print('Accel_Brake_Derive failed as the input was not recognised')
        return
    # Initialising DataFrames of the outputs
    AccelPos = pd.DataFrame()
    BrakePos = pd.DataFrame()

    Tract = [0]*len(SpeedTrace)
    for i in range(0,Runs): # Across all runs
        if DataFrame == 1:
            TestData = SpeedTrace.loc[:,('run'+str(i+1))] # Standard naming
            TestData = TestData.values # Converting from DataFrame to vector for ease
        elif Runs > 1:
            TestData = SpeedTrace[:,i] # Treated as a single vector
        else:
            TestData = SpeedTrace
        TestData = TestData * 0.277778  # Converting to m/s.
        TestAccelPos = [0]*len(TestData) # Output initialisation
        TestBrakePos = [0]*len(TestData) # Output initialisation
        for x in range(1,len(TestData)): # Calculating traction
            Speed = ((TestData[x] + TestData[x - 1]) / 2) # Mid-point speed
            TractA = Mass*(TestData[x] - TestData[x - 1]) # Effort for acceleration
            TractD = 0.5 * 0.2 * 1.1 * 1.225 * Speed ** 2 # Overcoming drag
            TractR = Mass * 9.81 * (0.005*Speed + 0.00005)  # Rolling Resistances
            Tract[x] = (TractA + TractD + TractR)*Speed # Power
            if Tract[x] > 0:
                TestAccelPos[x] = 100*(Tract[x]/BaseVehiclePower) # % Accelerator
            elif Tract[x] < 0:
                TestBrakePos[x] = -100*(Tract[x]/BaseVehiclePower) # % Brake
        AccelPos[('run' + str(i + 1))] = TestAccelPos
        BrakePos[('run' + str(i + 1))] = TestBrakePos
    if outputs == 1:
        return AccelPos
    elif outputs == 2:
        return AccelPos , BrakePos
    else:
        print('Outputs for Accel_Brake_Derive incorrectly specified. Please choose either 1 or 2. The effect of output can be seen on line 51 of the file')

