toPlot = [20, 60, 2.4, .000436, .0094; 20, 60, 1.7, .0003, .0091; 40, 60, 9.5, .000436, .0094; 40, 60, 6.5, .0003, .0091];
names = {'LBW-X', 'LBW-Y', 'HBW-X', 'HBW-Y'};

%% 1
for i = 1:4
    B1(toPlot(i, 1),toPlot(i, 2),toPlot(i, 3),toPlot(i, 4),toPlot(i, 5),names{i});
end

%% 2
clf;
i = 1;
B2_open_z(toPlot(i, 1),toPlot(i, 2),toPlot(i, 3),toPlot(i, 4),toPlot(i, 5));
i = 3;
B2_open_z(toPlot(i, 1),toPlot(i, 2),toPlot(i, 3),toPlot(i, 4),toPlot(i, 5));
title('Open loop sys of X axis with LBW(blue) and HBW(red) controllers in z domain');
saveas(gcf, 'qB2-1.png');
clf;
i = 1;
B2_close_s(toPlot(i, 1),toPlot(i, 2),toPlot(i, 3),toPlot(i, 4),toPlot(i, 5));
i = 3;
B2_close_s(toPlot(i, 1),toPlot(i, 2),toPlot(i, 3),toPlot(i, 4),toPlot(i, 5));
title('Closed loop sys of X axis with LBW(blue) and HBW(red) controllers in s domain');
saveas(gcf, 'qB2-2.png');

%% 3
table = [];
for i = 1:4
    info1 = B3_z(toPlot(i, 1),toPlot(i, 2),toPlot(i, 3),toPlot(i, 4),toPlot(i, 5),names{i});
    info2 = B3_s(toPlot(i, 1),toPlot(i, 2),toPlot(i, 3),toPlot(i, 4),toPlot(i, 5),names{i});
    table = [table; info1; info2];
end
disp(table);

%% functions
function B1(w, phi, mag, Je, Be, name)
    Ke = 1.59;
    Ka = 1;
    Kt = .49;
    T = .0001;

    w = 2*pi*w; % hz -> rad/s
    phi = phi * pi/180; % deg -> rad
    K = 10^(-mag/20); % found that with K=1, mag at w is -22.4dB, so we need to shift by +22.4dB
    a = (1+sin(phi))/(1-sin(phi));
    t = 1/(sqrt(a)*w);
    C = tf([K*a*t K], [t 1]);

    %{
    disp(a);
    disp(t);
    disp(K);
%}
    Ki = w/10;
    G = tf([1 Ki], [1 0]);
    %disp(Ki);

    H = tf((Ke*Ka*Kt),[Je Be 0]);   % continuous time
    Hd = c2d(H,T);                  % discrete time
    clf;
    hold on;
    bode(Hd);

    % a
    CHd = c2d(C*H, T);
    bode(CHd);

    % b
    GCHd = c2d(G*C*H, T);
    bode(GCHd);
    
    title(['Open loop bode of ' name ' in z domain']);
    text1 = ['Blue=Regular' newline 'Red=With lead-lag' newline 'Yellow=With Integrator-lead-lag'];
    text2 = ['a=' strtrim(string(a)) 'T=' strtrim(string(t)) 'K=' string(K) 'Ki=' string(Ki)];
    annotation('textbox', [0.5, 0.2, 0.1, 0.1], 'String', text1);
    annotation('textbox', [0.7, 0.8, 0.1, 0.1], 'String', text2);
    saveas(gcf, ['qB1-' name '.png']);
end

function B2_open_z(w, phi, mag, Je, Be)
    Ke = 1.59;
    Ka = 1;
    Kt = .49;
    T = .0001;

    w = 2*pi*w; % hz -> rad/s
    phi = phi * pi/180; % deg -> rad
    K = 10^(-mag/20); % found that with K=1, mag at w is -22.4dB, so we need to shift by +22.4dB
    a = (1+sin(phi))/(1-sin(phi));
    t = 1/(sqrt(a)*w);
    C = tf([K*a*t K], [t 1]);

    Ki = w/10;
    G = tf([1 Ki], [1 0]);

    H = tf((Ke*Ka*Kt),[Je Be 0]);   % continuous time
    hold on;
    
    % b
    GCHd = c2d(G*C*H, T);
    bode(GCHd);
end

function B2_close_s(w, phi, mag, Je, Be)
    Ke = 1.59;
    Ka = 1;
    Kt = .49;
    T = .0001;

    w = 2*pi*w; % hz -> rad/s
    phi = phi * pi/180; % deg -> rad
    K = 10^(-mag/20); % found that with K=1, mag at w is -22.4dB, so we need to shift by +22.4dB
    a = (1+sin(phi))/(1-sin(phi));
    t = 1/(sqrt(a)*w);
    C = tf([K*a*t K], [t 1]);

    Ki = w/10;
    G = tf([1 Ki], [1 0]);

    H = tf((Ke*Ka*Kt),[Je Be 0]);   % continuous time
    hold on;
    
    % b
    GCH = G*C*H;
    bode(feedback(GCH,1));
end

function info = B3_z(w, phi, mag, Je, Be, name)
    Ke = 1.59;
    Ka = 1;
    Kt = .49;
    T = .0001;

    w = 2*pi*w; % hz -> rad/s
    phi = phi * pi/180; % deg -> rad
    K = 10^(-mag/20); % found that with K=1, mag at w is -22.4dB, so we need to shift by +22.4dB
    a = (1+sin(phi))/(1-sin(phi));
    t = 1/(sqrt(a)*w);
    C = tf([K*a*t K], [t 1]);

    Ki = w/10;
    G = tf([1 Ki], [1 0]);

    clf;
    hold on;
    
    H = tf((Ke*Ka*Kt),[Je Be 0]);   % continuous time
    Hd = c2d(H,T,'zoh');                  % discrete time
    bode(feedback(Hd, 1));

    % a
    CHd = c2d(C, T, 'tustin')*Hd;
    bode(feedback(CHd, 1));

    % b
    GCHd = c2d(G*C, T, 'tustin')*Hd;
    bode(feedback(GCHd, 1));
    
    sys = feedback(GCHd, 1);
    bw = string(bandwidth(sys));
    ze = mat2str(zero(sys),2);
    po = mat2str(pole(sys), 2);
    
    title(['Close loop bode of ' name ' in z domain']);
    text1 = ['Yellow=With Integrator-lead-lag' 'bandwidth(rad/s)=' bw 'zero:' ze 'pole:' po];
    text2 = ['a=' strtrim(string(a)) 'T=' strtrim(string(t)) 'K=' string(K) 'Ki=' string(Ki)];
    annotation('textbox', [0.6, 0.4, 0.1, 0.1], 'String', text1);
    annotation('textbox', [0.7, 0.8, 0.1, 0.1], 'String', text2);
    saveas(gcf, ['qB3-z-' name '.png']);
    
    clf;
    sys = feedback(GCHd, 1);
    step(sys);
    S = stepinfo(sys);
    annotation('textbox', [0.6, 0.2, 0.1, 0.1], 'String', strucToStr(S));
    title(['Close loop bode of ' name ' in z domain, step response']);
    saveas(gcf, ['qB3-z-step-' name '.png']);
    
    info = {name, bw, ze, po, string(S.RiseTime), string(S.Overshoot)};
end


function info = B3_s(w, phi, mag, Je, Be, name)
    Ke = 1.59;
    Ka = 1;
    Kt = .49;
    T = .0001;

    w = 2*pi*w; % hz -> rad/s
    phi = phi * pi/180; % deg -> rad
    K = 10^(-mag/20); % found that with K=1, mag at w is -22.4dB, so we need to shift by +22.4dB
    a = (1+sin(phi))/(1-sin(phi));
    t = 1/(sqrt(a)*w);
    C = tf([K*a*t K], [t 1]);

    Ki = w/10;
    G = tf([1 Ki], [1 0]);

    H = tf((Ke*Ka*Kt),[Je Be 0]);   % continuous time
    clf;
    hold on;
    bode(feedback(H, 1));
    bode(feedback(C*H, 1));
    bode(feedback(G*C*H, 1));
    
    sys = feedback(G*C*H, 1);
    bw = string(bandwidth(sys));
    ze = mat2str(zero(sys),2);
    po = mat2str(pole(sys), 2);
    
    title(['Close loop bode of ' name ' in s domain']);
    text1 = ['Yellow=With Integrator-lead-lag' 'bandwidth(rad/s)=' string(bw) 'zero:' mat2str(ze,2) 'pole:' mat2str(po,2)];
    text2 = ['a=' strtrim(string(a)) 'T=' strtrim(string(t)) 'K=' string(K) 'Ki=' string(Ki)];
    annotation('textbox', [0.6, 0.4, 0.1, 0.1], 'String', text1);
    annotation('textbox', [0.7, 0.8, 0.1, 0.1], 'String', text2);
    saveas(gcf, ['qB3-s-' name '.png']);
    
    clf;
    sys = feedback(G*C*H, 1);
    step(sys);
    S = stepinfo(sys);
    annotation('textbox', [0.6, 0.2, 0.1, 0.1], 'String', strucToStr(S));
    title(['Close loop bode of ' name ' in s domain, step response']);
    saveas(gcf, ['qB3-s-step-' name '.png']);
    
    info = {name, bw, ze, po, string(S.RiseTime), string(S.Overshoot)};
end

function str = strucToStr(struc)
    str = ['RiseTime(s): ' string(struc.RiseTime) 'Overshoot(%): ' string(struc.Overshoot)];
end
