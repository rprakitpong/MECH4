%% Matlab Code for FRF a 2nd order (Single Degree of Freedom) system 
% Prof. YUSUF ALTINTAS
% University of British Columbia  
% Mechanical Engineering Department 
% www.mal.mech.ubc.ca 
%  September 2020
clc;
clear all;
close all;

% Parameters of System
wn      = 100;          % natural freq [Hz]
wnr=wn*2*pi;            % natural freq [rad/s]
kz=1E7 ;                % stiffness[N/m]
eta     = 0.05;         % damping ratio
wd=wn*sqrt(1-eta^2);
wmin=0.01*wn; 
wmax=2*wn;
F0=1 ; %[N] External force amplitude 
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
%%  Transfer Function of Plant
% Modeling of the drive  in s domain%

sdof=tf([wnr^2/kz],[1 2*eta*wnr wnr^2]);  % Transfer function of a 2nd order system

%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
%% Bode plot (Frequency Repsonse of the system)
figure(100)
h=bodeplot(sdof);
grid;
setoptions(h,'FreqUnits','Hz','MagUnits','abs','Magscale','linear','FreqScale','linear','XLim',{[wmin wmax]});

grid
title(sprintf('Bode diagram of second order, underdamped system \n wn=%4.4g (Hz), Damping ratio=%4.4g',wn,eta));

 t = 0:0.00001:1;
figure(104); clf; 
zoom on;
[zstep,Time]=step(sdof,t);					% show step response
plot(Time,zstep*F0*10^6)
axis([0 0.2 0 0.2]);grid;
xlabel(' Time [s]'); ylabel('Displacement z[x E7[m]');
%title(sprintf('Step response of a second order, underdamped system \n wn[Hz]=%4.4g, Damping ratio=%4.4g, Stiffness[N/m]=%6.4g',wn,eta,kz));
%-------------------------------------------------------------------
  
 figure(106); clf;
 F1 =F0*sin(0.1*wnr*t);  z1=lsim(sdof,F1,t) ; plot(t,F1,t,z1*10^7);
axis([0.1 1 -1.2 1.2]);grid;
title(sprintf('wnr=628rad/s,Sinusoidal response for \n z(t)=sin(%4.4g t), wnd=(',0.1*wnr));
xlabel(' Time [s]'); ylabel('F(t)- Input,z(t)-Output');
legend('Force', 'Vibration');grid;

 figure(108); clf;
 F2 =F0*sin(wnr*t);  z2=lsim(sdof,F2,t) ; plot(t,F2,t,z2*10^7);
axis([0.1 0.22 -10 10])
title(sprintf('wn=628rad/s, Sinusoidal response for \n F(t)=sin(%4.4g t)',wnr));
xlabel(' Time [s]'); ylabel('F(t)- Input, z(t)-Output');
legend('Force', 'Vibration');grid;
 figure(110); clf;
 F3 = F0*sin(2*wnr*t);  z3=lsim(sdof,F3,t) ; plot(t,F3,t,z3*10^7);
axis([0.1 0.15 -1.2 1.2]);grid;
title(sprintf('wn=628rad/s, Sinusoidal response for \n F(t)=sin(%4.4g t) ',2*wnr));
xlabel(' Time [s]'); ylabel('F(t)- Input,z(t)-Output');
legend('Force', 'Vibration');grid;

