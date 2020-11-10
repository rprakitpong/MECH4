% p controller 
pController
scopeConfig = get_param('pController/Scope','ScopeConfiguration');
scopeConfig.DataLogging = true;
scopeConfig.DataLoggingSaveFormat = 'Dataset';
out = sim('pController');
printRiseAndOvershoot('p controller', out);

% LL controller
% from prelab q5a
% a = 13.9282
% T = 7.1074e-04 = .00071074
% K = 13.1800
llController
scopeConfig = get_param('llController/Scope','ScopeConfiguration');
scopeConfig.DataLogging = true;
scopeConfig.DataLoggingSaveFormat = 'Dataset';
out = sim('llController');
printRiseAndOvershoot('ll controller', out);

% LLI controller
% from prelab q5b
% Ki = 37.7000
lliController
scopeConfig = get_param('lliController/Scope','ScopeConfiguration');
scopeConfig.DataLogging = true;
scopeConfig.DataLoggingSaveFormat = 'Dataset';
out = sim('lliController');
printRiseAndOvershoot('lli controller', out);

function printRiseAndOvershoot(name, out)
    x1_data = out.ScopeData{1}.Values.Data(:,2);
    x1_time = out.ScopeData{1}.Values.Time;

    % rise time = 10% to 90%
    steadystate = x1_data(25001,1);
    [~,idx] = min(abs(x1_data-steadystate*0.9));
    p90 = x1_time(idx);
    [~,idx] = min(abs(x1_data-steadystate*0.1));
    p10 = x1_time(idx);
    risetime = p90 - p10;

    % overshoot = max - steady state
    maxVal = max(x1_data);
    overshoot = maxVal - steadystate;

    disp(name);
    disp(risetime);
    disp(overshoot);
end