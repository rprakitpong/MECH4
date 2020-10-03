% matlab 2020 b
% set up reference table
freq2amp = [];
freq2amp(1) = .2;
freq2amp(2) = .2;
freq2amp(3) = .2;
freq2amp(4) = .2;
freq2amp(5) = .2;
freq2amp(6) = .5;
freq2amp(7) = .5;
freq2amp(8) = .5;
freq2amp(9) = .5;
freq2amp(10) = .5;
freq2amp(20) = .9;
freq2amp(30) = .9;
freq2amp(40) = .9;
freq2amp(50) = .9;
freq2amp(60) = .9;
freq2amp(70) = .9;

list = dir("*.mat");
allNames = {list.name};

% to store values for bode plot
freq_mag_phase = zeros(length(allNames), 3);

if (false) 
    % for getting frequency, magnitude, and phase for the first time
    for k = 1 : length(allNames)
       % load all data from each file
       name = allNames{k};
       freq = str2double(extractBefore(name, "Hz"));
       data = load(name);
       time = data.output.time;
       in = data.output.CH1in;
       out = data.output.CH1out;
       sig = data.output.CH1sig; % should be equal to in

       % get speed
       pos = out;
       speed = deriv(pos);

       % plot speed and Vin
       clf();
       hold on;
       plot(time, sig);
       plot(time, speed);
       legend("sig", "speed");

       % get peak values
       title(strcat("get sig, freq = ", num2str(freq)));
       [sig_x, sig_y] = ginput_withZoom();
       title(strcat("get speed, freq = ", num2str(freq)));
       [sp_x, sp_y] = ginput_withZoom();

       % calculate magnitude ratio
       ratio = abs(sp_y)/abs(sig_y);

       % calculate phase
       period = 1/freq;
       diff = sig_x - sp_x;
       phase = (diff/period)*360;

       % store values
       freq_mag_phase(k, 1) = freq;
       freq_mag_phase(k, 2) = ratio;
       freq_mag_phase(k, 3) = phase;
       disp(freq_mag_phase(k, :));
    end

    freq_mag_phase = sortrows(freq_mag_phase);
    disp(freq_mag_phase);
end
% once you have frequency, magnitude, and phase values, you just initialize
% array with it
freq_mag_phase = [1.0000    0.7445  -26.8508;
    2.0000    0.7182  -51.7127;
    3.0000    0.7068  -64.6409;
    5.0000    0.0650  -59.6685;
    6.0000    0.0594  -69.6133;
    7.0000    0.0563  -83.5359;
    8.0000    0.0580  -79.5580;
    9.0000    0.0541  -81.3684;
   10.0000    0.0504  -98.1819;
   20.0000    0.0399  -49.7238;
   40.0000    0.0356  -76.7925;
   50.0000    0.0372  -82.9302;
   70.0000    0.0374 -105.2408];

% bode plot based on values from A2 and B4
Be = 0.055393;
Je = 7;
s = tf('s');
H = (.72*.887)/(Je*s + Be);
[mag, phase, wout] = bode(H);
clf();
bode(H);
saveas(gcf, "bode.png");

% plot magnitude
clf();
hold on;
mag = squeeze(mag(1,1,:));
plot(wout, mag);
scatter(freq_mag_phase(:, 1), freq_mag_phase(:, 2));
set(gca, 'XScale', 'log');
title("Bode plot");
xlabel("Frequency");
ylabel("Magnitude");
saveas(gcf, "bode-mag.png");

% plot phase
clf();
hold on;
phase = squeeze(phase(1,1,:));
plot(wout, phase);
scatter(freq_mag_phase(:, 1), freq_mag_phase(:, 3));
set(gca, 'XScale', 'log');
title("Bode plot");
xlabel("Frequency");
ylabel("Phase");
saveas(gcf, "bode-phase.png");

% ginput wrapper that lets user zoom
% [ to zoom out
% ] to zoom in
% enter to return
function [X, Y] = ginput_withZoom()
   X = 0; Y = 0;
   while 0<1
    [x,y,b] = ginput(1); 
    if isempty(b)
        break;
    elseif b==91
        ax = axis; width=ax(2)-ax(1); height=ax(4)-ax(3);
        axis([x-width/2 x+width/2 y-height/2 y+height/2]);
        zoom(1/2);
    elseif b==93
        ax = axis; width=ax(2)-ax(1); height=ax(4)-ax(3);
        axis([x-width/2 x+width/2 y-height/2 y+height/2]);
        zoom(2);    
    else
        X=x;
        Y=y;
    end
   end
end
