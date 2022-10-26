import { Home } from "./components/Home";
import { Counter } from "./components/Counter";
import { FetchData } from "./components/FetchData";
import { StudentAppointments } from "./components/StudentAppointments";
import { StudentLookForTutor } from "./components/StudentLookForTutor";
import { StudentHelp } from "./components/StudentHelp";
import { StudentAccount } from "./components/StudentAccount";

import { TutorAppointments } from "./components/TutorAppointments";
import { TutorMessageRequests } from "./components/TutorMessageRequests";
import { TutorAccount } from "./components/TutorAccount";
import { AdminCheckStats } from "./components/AdminCheckStats";
import { AdminUserMsg } from "./components/AdminUserMsg";
import { AdminReportAcct } from "./components/AdminReportAcct";
import { AdminAccount } from "./components/AdminAccount";

const AppRoutes = [
  
      {
        path: '/counter',
        element: <Counter />
      },
      {
        path: '/fetch-data',
        element: <FetchData />
      },
      {
        index: '/home',
        element: <Home />
    },
    {
        path: '/student-appointments',
        element: <StudentAppointments />
    },
    {
        path: '/look-for-tutor',
        element: <StudentLookForTutor />
    },
    {
        path: '/student-help',
        element: <StudentHelp />
    },
    {
        path: '/student-account',
        element: <StudentAccount />
    },
    {
        path: '/tutor-appointments',
        element: <TutorAppointments />
    },
    {
        path: '/tutor-message-requests',
        element: <TutorMessageRequests />
    },
    {
        path: '/tutor-account',
        element: <TutorAccount />
    },
    {
        path: '/check-statistics',
        element: <AdminCheckStats />
    },
    {
        path: '/user-messages',
        element: <AdminUserMsg />
    },
    {
        path: '/reported-accounts',
        element: <AdminReportAcct />
    },
    {
        path: '/admin-account',
        element: <AdminAccount />
    }

];

export default AppRoutes;
