using System;

namespace SMDRReader
{
    /// <summary>
    /// http://www.devconnectprogram.com/fileMedia/download/d5d23aca-8ea9-471d-92e4-dd0ca37cac3f or
    /// O:\Technology\Suppliers\Avaya\avaya-ip-office-8_0-manager-guide.pdf
    /// 14.1 SMDR Fields
    /// </summary>
    public class SMDRRecord
    {
        public SMDRRecord(string csv)
        {
            var recordArray = csv.Split(",".ToCharArray());
            if (recordArray.Length != 30)
            {
                throw new ArgumentOutOfRangeException(string.Format("Expected 30 items, recieved {0}", recordArray.Length));
            }

            CallStart = DateTime.Parse(recordArray[0]);
            ConnectedTime = TimeSpan.Parse(recordArray[1]);
            RingTime = TimeSpan.FromSeconds(double.Parse(recordArray[2]));
            Caller = recordArray[3];
            CallDirection = recordArray[4];
            CalledNumber = recordArray[5];
            DialedNumber = recordArray[6];
            Account = recordArray[7];
            IsInternal = recordArray[8] == "1";
            CallId = long.Parse(recordArray[9]);
            Continuation = int.Parse(recordArray[10]);
            Party1Device = recordArray[11];
            Party1Name = recordArray[12];
            Party2Device = recordArray[13];
            Party2Name = recordArray[14];
            ExternalTargeterId = recordArray[15];
            HoldTime = TimeSpan.FromSeconds(double.Parse(recordArray[16]));
            double parktimeseconds = 0;
            if (double.TryParse(recordArray[17], out parktimeseconds))
            {
                ParkTime = TimeSpan.FromSeconds(parktimeseconds);
            }
            AuthValid = recordArray[18] == "1";
            AuthCode = recordArray[19];
            UserCharged = recordArray[20];
            CallCharge = recordArray[21];
            Currency = recordArray[22];
            AmountatLastUserChange = recordArray[23];
            CallUnits = recordArray[24];
            UnitsatLastUserChange = recordArray[25];
            CostperUnit = recordArray[26];
            MarkUp = recordArray[27];
            ExternalTargetingCause = recordArray[28];
            ExternalTargetedNumber = recordArray[29];
        }
        /// <summary>
        /// Call start time in the format YYYY/MM/DD HH:MM:SS. For all transferred call segment this is the time the call was
        /// initiated, so each segment of the call has the same call start time.
        /// </summary>
        public DateTime CallStart { get; set; }
        /// <summary>
        /// Duration of the connected part of the call in HH:MM:SS format. This does not include ringing, held and parked time. A
        /// lost or failed call will have a duration of 00:00:00. The total duration of a record is calculated as Connected Time + Ring
        /// Time + Hold Time + Park Time.
        /// </summary>
        public TimeSpan ConnectedTime { get; set; }
        /// <summary>
        /// Duration of the ring part of the call in seconds.
        /// <para>
        /// For inbound calls this represents the interval between the call arriving at the switch and it being answered, not the
        /// time it rang at an individual extension. 
        /// </para>
        /// For outbound calls, this indicates the interval between the call being initiated and being answered at the remote
        /// end if supported by the trunk type. Analog trunks are not able to detect remote answer and therefore cannot
        /// provide a ring duration for outbound calls.
        /// <para>
        /// </para>
        /// </summary>
        public TimeSpan RingTime;
        /// <summary>
        /// The callers' number. If the call was originated at an extension, this will be that extension number. If the call originated
        /// externally, this will be the CLI of the caller if available, otherwise blank.
        /// </summary>
        public string Caller { get; set; }
        /// <summary>
        /// Direction of the call – I for Inbound, O for outbound. Internal calls are represented as O for outbound. This field can be
        /// used in conjunction with Is_Internal below to determine if the call is internal, external outbound or external inbound.
        /// </summary>
        public string CallDirection { get; set; }
        /// <summary>
        /// This is the number called by the system. For a call that is transferred this field shows the original called number, not the
        /// number of the party who transferred the call. 
        ///     Internal calls: The extension, group or short code called. 
        ///     Inbound calls: The target extension number for the call. 
        ///     Outbound calls: The dialed digits. 
        ///     Voice Mail: Calls to a user's own voicemail mailbox.
        /// </summary>
        public string CalledNumber { get; set; }
        /// <summary>
        /// For internal calls and outbound calls, this is identical to the Called Number above. For inbound calls, this is the DDI of
        /// the incoming caller.
        /// </summary>
        public string DialedNumber { get; set; }
        /// <summary>
        /// The last account code attached to the call. Note: System account codes may contain alphanumeric characters.
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 0 or 1, denoting whether both parties on the call are internal or external (1 being an internal call). Calls to SCN
        /// destinations are indicated as internal.
        /// <para>
        /// Direction | Is Internal | Call Type
        /// -------------------------------------------------
        ///     I     |      0      | Incoming external call.
        ///     O     |      1      | Internal call.
        ///     O     |      0      | Outgoing external call.
        /// </para>
        /// </summary>
        public bool IsInternal { get; set; }
        /// <summary>
        /// This is a number starting from 1,000,000 and incremented by 1 for each unique call. If the call has generates several
        /// SMDR records, each record will have the same Call ID. Note that the Call ID used is restarted from 1,000,000 if the
        /// system is restarted. 
        /// </summary>
        public long CallId { get; set; }
        /// <summary>
        /// 1 if there is a further record for this call id, 0 otherwise. 
        /// </summary>
        public int Continuation { get; set; }
        /// <summary>
        /// The device 1 number. This is usually the call initiator though in some scenarios such as conferences this may vary. If an
        /// extension/hunt group is involved in the call its details will have priority over a trunk. That includes remote SCN
        /// destinations.
        /// <para>
        /// Type             | Party Device                                                | Party Name
        /// ----------------------------------------------------------------------------------------------------------------------------------------------------
        /// Internal Number  | E&lt;extension number&gt;                                   | &lt;name&gt;
        /// Voicemail        | V&lt;9500 + channel number&gt;                              | VM Channel &lt;channel number&gt;
        /// Conference       | V&lt;1&gt;&lt;conference number&gt;+&lt;channel number&gt;  | CO Channel &lt;conference number.channel number&gt;
        /// Line             | T&lt;9000+line number&gt;                                   | Line &lt;line number&gt;.&lt;channel if applicable&gt;
        /// Other            | V&lt;8000+device number&gt;                                 | U&lt;device class&gt; &lt;device number&gt;.&lt;device channel&gt;
        /// Unknown/Tone     | V8000                                                       | U1 0.0
        /// </para>
        ///
        /// </summary>
        public string Party1Device { get; set; }
        /// <summary>
        /// The name of the device – for an extension or agent, this is the user name.
        /// </summary>
        public string Party1Name { get; set; }
        /// <summary>
        /// The other party for the SMDR record of this call segment. See Party1Device above.
        /// </summary>
        public string Party2Device { get; set; }
        /// <summary>
        /// The other party for the SMDR record of this call segment. See Party1Name above.
        /// </summary>
        public string Party2Name { get; set; }
        /// <summary>
        /// The amount of time in seconds the call has been held during this call segment.
        /// </summary>
        public TimeSpan HoldTime { get; set; }
        /// <summary>
        /// The amount of time in seconds the call has been parked during this call segment.
        /// </summary>
        public TimeSpan ParkTime { get; set; }
        /// <summary>
        /// This field is used for authorization codes. This field shows 1 for valid authorization or 0 for invalid authorization
        /// </summary>
        public bool AuthValid { get; set; }
        /// <summary>
        /// This field shows either the authorization code used or n/a if no authorization code was used.
        /// </summary>
        public string AuthCode { get; set; }
        /// <summary>
        /// This and the following fields are used for ISDN Advice of Charge (AoC) . The user to which the call charge has been
        /// assigned. This is not necessarily the user involved in the call. 
        /// </summary>
        public string UserCharged { get; set; }
        /// <summary>
        /// The total call charge calculated using the line cost per unit and user markup.
        /// </summary>
        public string CallCharge { get; set; }
        /// <summary>
        /// The currency. This is a system wide setting set in the system configuration. 
        /// </summary>
        public string Currency { get; set; }
        /// <summary>
        /// The current AoC amount at user change.
        /// </summary>
        public string AmountatLastUserChange { get; set; }
        /// <summary>
        /// The total call units.
        /// </summary>
        public string CallUnits { get; set; }
        /// <summary>
        /// The current AoC units at user change.
        /// </summary>
        public string UnitsatLastUserChange { get; set; }
        /// <summary>
        /// This value is set in the system configuration against each line on which Advice of Charge signalling is set. The values are
        /// 1/10,000th of a currency unit. For example if the call cost per unit is £1.07, a value of 10700 should be set on the line.
        /// </summary>
        public string CostperUnit { get; set; }
        /// <summary>
        /// Indicates the mark up value set in the system configuration for the user to which the call is being charged. The field is in
        /// units of 1/100th, for example an entry of 100 is a markup factor of 1.
        /// </summary>
        public string MarkUp { get; set; }

        #region The following additional fields are provided by system SMDR. They are not provided by Delta Server SMDR
        /// <summary>
        /// This field indicates who or what caused the external call and a reason code. For example U FU indicates that the
        /// external call was caused by the Forward Unconditional setting of a User.
        /// <para>
        /// Targeted by                                | Reason Code
        /// ------------------------------------------------------------------------------------------------------------------
        /// HG   - Hunt Group.                         | fb   - Forward on Busy.
        /// U    - User.                               | fu   - Forward unconditional.
        /// LINE - Line.                               | fnr  - Forward on No Response.
        /// AA   - Auto Attendant.                     | fdnd - Forward on DND.
        /// ICR  - Incoming Call Route.                | CfP  - Conference proposal (consultation) call.
        /// RAS  - Remote Access Service.              | Cfd  - Conferenced.
        /// ?    - Other.                              | MT   - Mobile Twinning.
        ///                                            | TW   - Teleworker.
        ///                                            | XfP  - Transfer proposal (consultation) call. 
        ///                                            | Xfd  - Transferred call.
        /// </para>
        /// </summary>
        public string ExternalTargetingCause { get; set; }
        /// <summary>
        /// The associated name of the targeter indicated in the External Targeting Cause field. For hunt groups and users this will
        /// be their name in the system configuration. For an Incoming Call Route this will be the Tag if set, otherwise ICR.
        /// </summary>
        public string ExternalTargeterId { get; set; }
        /// <summary>
        /// This field is used for forwarded, Incoming Call Route targeted and mobile twin calls to an external line. It shows the
        /// external number called by the system as a result of the off switch targeting where as other called fields give the original
        /// number dialled.
        /// </summary>
        public string ExternalTargetedNumber { get; set; }
        #endregion
    }
}
