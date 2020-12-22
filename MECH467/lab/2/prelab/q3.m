Ke = 10/3.1415;
Ka = .887;
Kt = .72;
Je = .0007;
Be = .00612;
T = .0002;

H = tf((Ke*Ka*Kt),[Je Be 0]);   % continuous time
Hd = c2d(H,T);                  % discrete time

% a
rlocus(H);
saveas(gcf, 'q3a-cont.jpg');
rlocus(Hd);
saveas(gcf, 'q3a-disc.jpg');

% b
% can also use margin(H) instead of measuring manually
bode(H);
bode(Hd);

%c 
Hd_1 = c2d(H, 0.02);
Hd_2 = c2d(H, 0.002);
bode(Hd_1);
bode(Hd_2);