%% s
name = 'lli.mat';
lli = load(name);
time = lli.lli.X.Data;
x_act = lli.lli.Y(1).Data;
y_act = lli.lli.Y(2).Data;
x_ref = lli.lli.Y(3).Data;
y_ref = lli.lli.Y(4).Data;

name_ = {'lbw.mat', 'hbw.mat', 'mix.mat'};

ax_ = [13.9282, 13.9282, 13.9282]; %lbw, hbw, hbw
Tx_ = [0.0021323, 0.0010661, 0.0010661];
Kx_ = [0.75858, 0.33497, 0.33497];
Kix_ = [12.5664, 25.1327, 25.1327];

ay_ = [13.9282, 13.9282, 13.9282]; %lbw, hbw, lbw
Ty_ = [0.0021323, 0.0010661, 0.0021323];
Ky_ = [0.82224, 0.47315, 0.82224];
Kiy_ = [12.5664, 25.1327, 12.5664];
for i = 1:3
    % init variables to be loaded into simulink model
    T = 0.0001;
    Ka = 1;
    Kt = 0.49;
    Ke = 1.59;
    Jx = 0.000436;
    Bx = 0.0094;
    Jy = 0.0003;
    By = 0.0091;

    % use LLI controller
    ax = ax_(1, i);
    TTx = Tx_(1, i);
    Kx = Kx_(1, i);
    Kix = Kix_(1, i);

    LLx = tf([ax*TTx 1],[TTx 1]);
    Ix = tf([1 Kix],[1 0]);
    LLI_Lx_z = Kx*c2d(LLx*Ix, T, 'tustin');
    
    ay = ay_(1, i);
    TTy = Ty_(1, i);
    Ky = Ky_(1, i);
    Kiy = Kiy_(1, i);
  
    LLy = tf([ay*TTy 1],[TTy 1]);
    Iy = tf([1 Kiy],[1 0]);
    LLI_Ly_z = Ky*c2d(LLy*Iy, T, 'tustin');

    Tplot = time';
    xplot = x_ref';
    yplot = y_ref';
    
    sim('e2_sim');
    
    output = ans.sim;
    outputName = name_{i};
    save(sprintf(outputName), 'output');
    x_sim = ans.sim.Data(:,3);
    y_sim = ans.sim.Data(:,5);
    plot(x_sim, y_sim);
end