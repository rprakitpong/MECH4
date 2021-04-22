% q1
load('q1.mat');
a = LinearAnalysisToolProject.Plots.Variables.Value.A;
b = LinearAnalysisToolProject.Plots.Variables.Value.B;
c = LinearAnalysisToolProject.Plots.Variables.Value.C;
d = LinearAnalysisToolProject.Plots.Variables.Value.D;
system = ss(a,b,c,d);
plot = bodeplot(system);
plot.showCharacteristic('AllStabilityMargins');
p = getoptions(plot);
p.PhaseMatching = 'on';
p.PhaseMatchingFreq = .1;
p.PhaseMatchingValue = -90;
setoptions(plot,p);
% q2
w = 10000;
phi = 75; % deg
phi = phi * pi/180;
a = (1+sin(phi))/(1-sin(phi));
t = 1/(sqrt(a)*w);
K = 35000; % trial and error
Ki = w/10;
integrator = tf([1 Ki], [1 0]); % integrator
leadlag = tf([K*a*t K], [t 1]); % lead lag compensator
controller = leadlag*integrator;
system = controller*system;
% q3
plot = bodeplot(system);
plot.showCharacteristic('AllStabilityMargins');
p = getoptions(plot);
p.PhaseMatching = 'on';
p.PhaseMatchingFreq = .1;
p.PhaseMatchingValue = -90;
setoptions(plot,p);
setoptions(plot,'Xlim',[0.1,10000]);
% q4
closedloop = system/(1+system); % black's formula
step(closedloop);
opt = stepDataOptions('StepAmplitude',5);
step(closedloop, opt);
opt = stepDataOptions('StepAmplitude',15);
step(closedloop, opt);
info = stepinfo(closedloop);
