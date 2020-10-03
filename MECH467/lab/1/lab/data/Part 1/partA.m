% matlab 2020 b
mmps = [5 15 30 40 50 60 90 100 140 160 200]; % all the linear speeds for reference
posSig = zeros(length(mmps), 2); % to store calculated values
negSig = zeros(length(mmps), 2);

list = dir("*.mat");
allNames = {list.name};
for k = 1 : length(allNames)
   % load all data from each file
   name = allNames{k};
   speed = str2double(extractBefore(name, "_"));
   data = load(name);
   time = data.output.time;
   in = data.output.CH1in;
   out = data.output.CH1out;
   sig = data.output.CH1sig;
   
   % calculate speed and torque
   posSpeed = 2*pi*speed/20;
   negSpeed = -posSpeed;
   posTorque = max(sig)*(106.44/120)*.72;
   negTorque = min(sig)*(106.44/120)*.72;
   
   % store calculated values in array
   posSig(k, 1) = posSpeed;
   posSig(k, 2) = posTorque;
   negSig(k, 1) = negSpeed;
   negSig(k, 2) = negTorque;
end

% plot the calculated values
[m, b] = plotter_indv(posSig(:, 1), posSig(:, 2), "posSig.png");
[m, b] = plotter_indv(negSig(:, 1), negSig(:, 2), "negSig.png");
plotter_both(posSig(:, 1), posSig(:, 2), negSig(:, 1), negSig(:, 2), "sig.png");

% wrapper function to plot either pos or neg values
function [m, b] = plotter_indv(x, y, name)
    clf();    
    [m, b] = plotter(x, y, name);
    nums = strcat("slope = ", num2str(m), ", y-int = ", num2str(b));
    title(strcat("Speed (rad/s) vs Torque (Nm) where ", nums));
    saveas(gcf, name);
end

% wrapper function to plot both pos and neg values
function plotter_both(x1, y1, x2, y2, name)
    clf();
    plotter(x1, y1, name);
    plotter(x2, y2, name);
end

% helper function to plot values
function [m, b] = plotter(x, y, name)
    disp(sortrows([x, y]));
    scatter(x, y);
    %{
    x_text = cellstr(num2str(x));
    y_text = cellstr(num2str(y));
    texts = strcat(x_text, y_text);
    text(x + .5, y, texts', 'Fontsize', 7); 
    %}
    hold on;
    m = x\y;
    p = polyfit(x, y, 1);
    b = p(2);
    yCalc = m*x;
    plot(x, yCalc);
    title("Speed (rad/s) vs Torque (Nm)");
    xlabel("Speed (rad/s)");
    ylabel("Torque (Nm)");
    saveas(gcf, name);
end