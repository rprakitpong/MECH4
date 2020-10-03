% matlab 2020 b
list = dir("*.mat");
allNames = {list.name};
for k = 1 : length(allNames)
   % load all data from each file
   name = allNames{k};
   data = load(name);
   time = data.output.time;
   in = data.output.CH1in;
   out = data.output.CH1out;
   sig = data.output.CH1sig; % should be equal to in
   
   % values given in lab manual
   amp = 3;
   Ts = 0.1;
   samplingTime = .005; % difference of each time value
   
   % values from part A
   uk = 0.23435; 
   Be = 0.055393;
   
   % value to guess and check, edit here
   Je = 7; 
   
   % filter pos values and convert to speed and accel
   pos = out;
   [B, A] = butter(4, 99*2*samplingTime); % change 100Hz cut off to 99Hz to make butter work
   filtfilt(A, B, pos);
   speed = deriv(pos);
   accel = deriv(speed);
   
   % calculate lhs and rhs
   lhs = .887*.72*sig - Be*speed - uk*sign(speed);
   rhs = Je*accel;
   
   % plot
   clf();
   hold on;
   plot(time(98:362), lhs(98:362)); % only plot parts where screw is active
   plot(time(98:362), rhs(98:362));
   title(strcat("Torque balance of equation where Je = ", num2str(Je)));
   ylabel("Torque (Nm)");
   xlabel("Time (s)");
   legend("LHS", "RHS");
   saveas(gcf, "fig.png");
end