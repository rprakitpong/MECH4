clf;
% q1
a = load('MaglevPlant.mat');
x = a.Plant_frf(:,1);
y = a.Plant_frf(:,2);
gain = abs(y);
theory_phase = angle(y)*180/pi;
hold on;
set(gca, 'XScale', 'log', 'YScale', 'log');
loglog(x,gain);
%loglog(x,phase);
P = tf([1],[.001 0 -2.5],'InputDelay',.0001); 
options = bodeoptions;
options.FreqUnits = 'Hz';
options.MagScale = 'log';
options.MagUnits = 'abs';
options.Xlim = [10,1000];
%bode(P,options);
[mag,theory_phase,wout] = bode(P,options);
theory_phase = squeeze(theory_phase)+360;
theory_gain = squeeze(mag);
%loglog(wout,squeeze(mag));
%loglog(wout,phase);
hold off;
%{
% tried this, didnt work, guess matched only 7%
guess = tfest(a.Plant_frf, 4, 2, .0001);
options = bodeoptions;
options.FreqUnits = 'Hz';
options.MagScale = 'log';
options.MagUnits = 'abs';
options.Xlim = [10,1000];
bode(guess,options);
%}
% q2
pzmap(P);
% q3
C = tf([1 1000],[1]);
L = P*C;
closedloop = L/(1+L);
% q4
margin(L);
% q5,6
nyquist(L);
% q7
step(closedloop);
