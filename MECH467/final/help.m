%% set up
Ka = .887;
Kt = .72;
J = 7*10^(-4);
B = .006;
Ke = 3.18;
T = 1/1000;
Gs = tf([Ke*Kt*Ka], [J B 0]);
Gz = c2d(Gs, T, 'zoh');
%% bode and margin
bode(Gz);
margin(Gz);
% proportional ctrl: gain crossover freq of w (deg):
% 1. get mag at w
% 2. K = 1/mag at w
wg = 100; % deg
[mag_g, ~] = bode(Gz, wg);
Kg = 1/mag_g;
G1 = Gz*Kg;
% phase margin of PM at freq of w
% 1. get phase at w
% 2. phi = PM - (w + 180)
% 3. calc alpha and T
% 4. convert LLs to LLZ if needed
% 5. get K from LL*G
PM = 60;
wc = 300;
[~, phase_ll] = bode(Gz, wc);
phi = PM - (phase_ll + 180);
phi = pi*phi/180;
a = (1+sin(phi))/(1-sin(phi));
t = 1/(sqrt(a)*wc);
LLs = tf([a*t 1], [t 1]);
LLz = c2d(LLs, T, 'tustin');
[mag, ~] = bode(LLz*Gz, wc);
K = 1/mag;
LLs = K*LLs;
LLz = K*LLz;
%% nyquist
%nyquist(G);
%% root locus
%rlocus(G);