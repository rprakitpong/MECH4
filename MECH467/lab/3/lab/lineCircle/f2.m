%% low feedrate
name = 'lli.mat';
lli = load(name);
time = lli.lli.X.Data;
x_act = lli.lli.Y(1).Data;
y_act = lli.lli.Y(2).Data;
x_ref = lli.lli.Y(3).Data;
y_ref = lli.lli.Y(4).Data;

e_x_lowFR = x_ref - x_act;
e_y_lowFR = y_ref - y_act;

clf();
hold on;
plot(time, e_x_lowFR);
plot(time, e_y_lowFR);

%% high feedrate
% init variables to be loaded into simulink model
T = 0.0001;
Ka = 1;
Kt = 0.49;
Ke = 1.59;
Jx = 0.000436;
Bx = 0.0094;
Jy = 0.0003;
By = 0.0091;

% use LBW LLI controller
a = 13.9282;
T_ = 0.0021323;
Kx = 0.75858;
Ky = 0.82224;
Ki = 12.5664;

LL = tf([a*T_ 1],[T_ 1]);
I = tf([1 Ki],[1 0]);
LLI_Lx_z = Kx*c2d(LL*I, T, 'tustin');
LLI_Ly_z = Ky*c2d(LL*I, T, 'tustin');

name = 'f1.mat';
data = load(name);
Tplot = data.txy.t;
xplot = data.txy.x;
yplot = data.txy.y;

sim('e2_sim.slx');

t = ans.sim.Data(:,1);
e_x_hiFR = ans.sim.Data(:,2) - ans.sim.Data(:,3);
e_y_hiFR = ans.sim.Data(:,4) - ans.sim.Data(:,5);
plot(t, e_x_hiFR);
plot(t, e_y_hiFR);

%% plot formatting
legend('e_x low feedrate', 'e_y low feedrate', 'e_x high feedrate', 'e_y high feedrate');
title('Error over time');
xlabel('Time (s)');
saveas(gcf, 'F2.png');
ylabel('Error (mm)');