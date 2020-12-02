%% init vars
clf();

A = 1000; % mm/s^2
fc = 250; % mm/s
T = 0.1; % ms
T = T * 0.001; % s

P1 = [0 0];
P2 = [40 30];
P3 = [60 30];
P4 = [90 30];

%% P1 -> P2
[T1, T2, T3] = calc(50); % sqrt(30*30+40*40)=50
T_total = T1+T2+T3;

x_ratio = 4/5;
y_ratio = 3/5;

t = 0;
s = 0;
sdot = 0;
sdotdot = A;
xr = 0;
yr = 0;
vxr = 0;
vyr = 0;
axr = A*x_ratio;
ayr = A*y_ratio;

count = 1;
data1 = zeros(T_total/T, 10);
while t <= T_total-T
    data1(count, 1) = t;
    data1(count, 2) = s;
    data1(count, 3) = sdot;
    data1(count, 4) = sdotdot;
    data1(count, 5) = xr;
    data1(count, 6) = yr;
    data1(count, 7) = vxr;
    data1(count, 8) = vyr;
    data1(count, 9) = axr;
    data1(count, 10) = ayr;

    if t < T1
        sdotdot = A;
    elseif t >= T1 && t < T1+T2
        sdotdot = 0;
    else
        sdotdot = -A;
    end
    
    sdot = sdot + T*sdotdot;
    s = s + sdot*T;
    
    xr = s*x_ratio;
    yr = s*y_ratio;
    vxr = sdot*x_ratio;
    vyr = sdot*y_ratio;
    axr = sdotdot*x_ratio;
    ayr = sdotdot*y_ratio;
    
    t = t + T;
    count = count + 1;
end

%% P2 -> P3
[T1, T2, T3] = calc(20); % 60-40=20
T_total = T1+T2+T3;

x_ratio = 1;
y_ratio = 0;

t = 0;
s = 0;
sdot = 0;
sdotdot = A;
xr = 0;
yr = 0;
vxr = 0;
vyr = 0;
axr = A*x_ratio;
ayr = A*y_ratio;

count = 1;
data2 = zeros(T_total/T, 10);
while t <= T_total-T
    data2(count, 1) = t+data1(end, 1);
    data2(count, 2) = s+data1(end, 2);
    data2(count, 3) = sdot+data1(end, 3);
    data2(count, 4) = sdotdot;
    data2(count, 5) = xr+data1(end, 5);
    data2(count, 6) = yr+data1(end, 6);
    data2(count, 7) = vxr+data1(end, 7);
    data2(count, 8) = vyr+data1(end, 8);
    data2(count, 9) = axr;
    data2(count, 10) = ayr;

    if t < T1
        sdotdot = A;
    elseif t >= T1 && t < T1+T2
        sdotdot = 0;
    else
        sdotdot = -A;
    end
    
    sdot = sdot + T*sdotdot;
    s = s + sdot*T;
    
    xr = s*x_ratio;
    yr = s*y_ratio;
    vxr = sdot*x_ratio;
    vyr = sdot*y_ratio;
    axr = sdotdot*x_ratio;
    ayr = sdotdot*y_ratio;
    
    t = t + T;
    count = count + 1;
end


%% P3 circle
L = 2*3.1415*30;
[T1, T2, T3] = calc(L); % 90-60=30
T_total = T1+T2+T3;

t = 0;
s = 0;
sdot = 0;
sdotdot = A;
xr = 0;
yr = 0;
vxr = 0;
vyr = 0;
axr = A*x_ratio;
ayr = A*y_ratio;

count = 1;
data3 = zeros(T_total/T, 10);
while t <= T_total-T
    if t < T1
        sdotdot = A;
    elseif t >= T1 && t < T1+T2
        sdotdot = 0;
    else
        sdotdot = -A;
    end
    
    sdot = sdot + T*sdotdot;
    s = s + sdot*T;
    
    [x_ratio, y_ratio] = getRatios(s/L);
    
    vxr_prev = vxr;
    vyr_prev = vyr;
    vxr = sdot*x_ratio;
    vyr = sdot*y_ratio;
    
    R = 30;
    xr = xr + vxr*T;
    yr = yr + vyr*T;
    
    axr = (vxr - vxr_prev)/T;
    ayr = (vyr - vyr_prev)/T;
    
    data3(count, 1) = t+data2(end, 1);
    data3(count, 2) = s+data2(end, 2);
    data3(count, 3) = sdot+data2(end, 3);
    data3(count, 4) = sdotdot;
    data3(count, 5) = xr+data2(end, 5);
    data3(count, 6) = yr+data2(end, 6);
    data3(count, 7) = vxr+data2(end, 7);
    data3(count, 8) = vyr+data2(end, 8);
    data3(count, 9) = axr;
    data3(count, 10) = ayr;
    
    t = t + T;
    count = count + 1;
end

%% data for plot
data = [data1; data2; data3];

%% plot
plot(data(:, 5), data(:, 6));
title('F1: x and y position');
xlabel('x position (mm)');
ylabel('y position (mm)');
saveas(gcf, 'qF1-1.png');
clf;
txy.t = data(:, 1);
txy.x = data(:, 5);
txy.y = data(:, 6);
save f1 txy

subplot(3,1,1);
plot(data(:,1), data(:,2));
title('F1: displacement (mm) over time (s)');
subplot(3,1,2);
plot(data(:,1), data(:,3));
title('F1: feedrate (mm/s) over time (s)');
subplot(3,1,3);
plot(data(:,1), data(:,4));
title('F1: tangential acceleration (mm/s/s) over time (s)');
saveas(gcf, 'qF1-2.png');
clf;

subplot(3,1,1);
hold on;
plot(data(:,1), data(:,5));
plot(data(:,1), data(:,6));
title('F1: axis position (mm) over time (s)');
legend('x', 'y');
subplot(3,1,2);
hold on;
plot(data(:,1), data(:,7));
plot(data(:,1), data(:,8));
title('F1: axis velocity (mm/s) over time (s)');
legend('x', 'y');
subplot(3,1,3);
hold on;
plot(data(:,1), data(:,9));
plot(data(:,1), data(:,10));
title('F1: axis acceleration (mm/s/s) over time (s)');
legend('x', 'y');
saveas(gcf, 'qF1-3.png');
clf;

%% helper
function [x_r, y_r] = getRatios(r)
    x_r = 0;
    y_r = 0;
    if r < .25
        theta = (pi/2)*(r/.25);
        x_r = sin(theta);
        y_r = -cos(theta);
    elseif r >= .25 && r < 0.5
        theta = (pi/2)*((r-0.25)/.25);
        x_r = cos(theta);
        y_r = sin(theta);
    elseif r >= 0.5 && r < 0.75
        theta = (pi/2)*((r-0.5)/.25);
        x_r = -sin(theta);
        y_r = cos(theta);
    else
        theta = (pi/2)*((r-0.75)/.25);
        x_r = -cos(theta);
        y_r = -sin(theta);
    end
end

function [T1, T2, T3] = calc(L)
    A = 1000; % mm/s^2
    fc = 250; % mm/s
    T = 0.1; % ms
    T = T * 0.001; % s

    T1 = fc/A;
    T3 = T1;
    s_init = A*T1*T1/2; % this is 20
    T2 = (L-2*s_init)/fc;
    T2 = ceil(T2/T)*T;
    if T2 < 0
        T2 = 0;
        s_init = L/2;
        T1 = sqrt(2*s_init/A);
        T1 = ceil(T1/T)*T;
        T3 = T1;
    end
end
