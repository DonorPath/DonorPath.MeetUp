var demoTimes;

function FetchDemoTimes() {
    var rootUri = location.toString();
    if (location.pathname != '/')
        rootUri = rootUri.replace(location.pathname, '/');

    var appointmentUri = rootUri + 'home/appointments';
    $.getJSON( appointmentUri)
      .done(function (data) {
          demoTimes = $.map(data, function (e) { return moment(e); });
          BindDemoTimes();
          EnableForm();
      });
}

function EnableForm()
{
    $('#MeetupLoad').hide();
    $('#appointmentSubmit').attr('disabled', false);
    $('#AppointmentTime').attr('disabled', false);
    $('#Email').attr('disabled', false);
}

function DisableForm()
{

    BindDemoTimes();
    $('#MeetupLoad').show();
    $('#appointmentSubmit').attr('disabled', true);
    $('#AppointmentTime').attr('disabled', true);
    $('#Email').attr('disabled', true);
}


function BindDemoTimes() {
    Date.parseDate = function (input, format) {
        return moment(input, format).toDate();
    };
    Date.prototype.dateFormat = function (format) {
        return moment(this).format(format);
    };
    if ($('#AppointmentTime').length > 0) {
        $("#AppointmentTime").datetimepicker({
            onGenerate: function (ct) {
                $('.xdsoft_date').each(function () {
                    var calDate = (parseInt($(this).attr('data-month')) + 1) + '/' + $(this).attr('data-date') + '/' + $(this).attr('data-year');
                    var result = $.grep(demoTimes, function (e) {
                        return e.format('l') === calDate;
                    });
                    if (result.length === 0) {
                        $(this).addClass('xdsoft_disabled');
                    }
                });
            },
            timepicker: false,
            formatTime: 'h:mm a',
            format: 'MM/DD/YYYY h:mm a',
            formatDate: 'MM/DD/YYYY',
            onChangeDateTime: function (c) {
                var formattedCurrentDate = moment(c).format("l"),
                    times = $.map(demoTimes, function (e, i) {
                        if (e.format('l') === formattedCurrentDate) return e.format('H:mm a');
                    });
                if (times.length > 0) {
                    this.setOptions({
                        timepicker: true,
                        allowTimes: times
                    });
                }
            }
        });
    }
}

function initGenericDateTimePicker() {
    if ($('.datetimepicker').length > 0) {
        $(".datetimepicker").datetimepicker({
            formatTime: 'h:mm a',
            format: 'MM/DD/YYYY h:mm a',
            formatDate: 'MM/DD/YYYY',
            step: 15
        });
    }
}

function ScheduledFailure()
{
    alert('BUG!');
    return false;
}

function ScheduledSuccess(context) {
    alert('All good!');
    return false;
}

function BeginSubmitAppointment()
{
    DisableForm();
}
    
function CompleteSubmitAppointment()
{
    EnableForm()
}