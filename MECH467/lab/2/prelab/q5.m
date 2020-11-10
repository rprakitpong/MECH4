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

disp(a);
disp(t);
disp(K);

Ki = w/10;
G = tf([1 Ki], [1 0]);
disp(Ki);

H = tf((Ke*Ka*Kt),[Je Be 0]);   % continuous time
Hd = c2d(H,T);                  % discrete time
hold on;
bode(Hd);

% a
CHd = c2d(C*H, T);
bode(CHd);

% b
GCHd = c2d(G*C*H, T);
bode(GCHd);
