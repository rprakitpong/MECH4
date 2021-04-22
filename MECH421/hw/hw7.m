% q1
wa = 2*pi*10^3;
ws = 10*pi*10^3;
m = 1;
Kf = 1;
Ga = tf(1,[1/wa 1]);
Gs = tf(1,[1/ws 1]);
Gm = tf(1,[m 0 0]);
fs = 10000;
T = 1/fs;
P = Ga*Kf*Gm*Gs;
bode(P);
% q2
Pdelay = P*tf([1],[1], 'InputDelay',T/2);
Pzoh = c2d(P, T, 'zoh');
hold on;
legend on;
bode(Pdelay);
bode(Pzoh);
% q3
w = 100*2*pi; % convert from hz to rad/s
phi = 45; % deg
phi = phi * pi/180;
a = 12; % guess and check
t = 1/(sqrt(a)*w);
Kp = 112202; % at 100Hz, this K make gain ~= 0db
Cs = tf([Kp*a*t Kp], [t 1]);
Cz = c2d(Cs, T, 'tustin');
bode(Cs);
bode(Cz);
