Ke = 10/3.1415;
Ka = .887;
Kt = .72;
Je = .0007;
Be = .00612;
T = .0002;

w = 377;
phi = 60; % deg
phi = phi * pi/180;
a = (1+sin(phi))/(1-sin(phi));
t = 1/(sqrt(a)*w);
K = 13.18; % found that with K=1, mag at w is -22.4dB, so we need to shift by +22.4dB
C = tf([K*a*t K], [t 1]);

Ki = w/10;
G = tf([1 Ki], [1 0]);

H = tf((Ke*Ka*Kt),[Je Be 0]);   % continuous time
CG = C*G;
CH = C*H;
CGH = C*G*H;

hold on;
bode(H);
bode(C);
bode(CG);
bode(CH);
bode(CGH);
saveas(gcf, 'q6.jpg');
