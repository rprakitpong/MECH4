wa = 2*pi*10^3;
ws = 10*pi*10^3;
m = 1;
Ga = tf(1,[1/wa 1]);
Gs = tf(1,[1/ws 1]);
Gm = tf(1,[m 0 0]);
fs = 10000;
T = 1/fs;
syms s;
DAC = tf([10],[1], 'InputDelay',T/2);
Kf = 1;
ADC = 0.1;
% a
P = DAC*Ga*Kf*Gm*Gs*ADC;
bode(P);
% b
w = 100*2*pi; % convert from hz to rad/s
phi = 45; % deg
phi = phi * pi/180;
a = 12; % guess and check
t = 1/(sqrt(a)*w);
Kp = 112202; % at 100Hz, this K make gain ~= 0db
C = tf([Kp*a*t Kp], [t 1]);
L = C*P;
[Gm,Pm,Wcg,Wcp] = margin(L);
disp(Pm);
disp(Kp);
disp(a);
disp(t);
options = bodeoptions;
options.FreqUnits = 'Hz';
bode(L, options);
grid on;
% c
Gxr = L/(1+L);
Gxd = Gm/(1+L);
step(Gxr);
step(Gxd);
Gxr_org = Gxr;
Gxd_org = Gxd;
% d
wi = 40*2*pi; % hz to rad/s
Ti = 1/wi;
H = tf([Ti 1],[Ti 0]);
C = H*C;
L = C*P;
bode(L, options);
[Gm,Pm,Wcg,Wcp] = margin(L);
disp(Pm);
grid on;
% e
Gxr = L/(1+L);
Gxd = Gm/(1+L);
step(Gxr);
step(Gxd);