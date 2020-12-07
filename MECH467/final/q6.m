KaKt = 15;
J = .001;
B = .005;
Ke = 10000;
Kd = 10;
Ts = .001;
G1s = tf([KaKt*Kd*Ke], [J B 0 0]);
G1z = c2d(G1s, Ts, 'zoh');
G2s = tf([KaKt*Kd*Ke], [J B 0 0], 'InputDelay', Ts);
G2z = c2d(G2s, Ts, 'zoh');
Gz = G1z - G2z;
[num, den] = tfdata(Gz);
celldisp(num);
celldisp(den);

disp('pole placement');
drm = 1.1; %damp ratio m
wm = 10;
s1 = (-2*drm*wm+sqrt(4*drm*drm*wm*wm-4*wm*wm))/2;
s2 = (-2*drm*wm-sqrt(4*drm*drm*wm*wm-4*wm*wm))/2;
Gz_ss = ss(Gz);
disp(Gz_ss.A);
disp('-');
disp(Gz_ss.B);
poleplace = place(Gz_ss.A, Gz_ss.B, [s1 s2]);