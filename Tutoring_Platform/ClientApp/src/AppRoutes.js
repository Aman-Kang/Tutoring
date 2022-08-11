import { Counter } from "./components/Counter";
import { FetchData } from "./components/FetchData";
import { StudentAppointments } from "./components/StudentAppointments";
import { StudentLookForTutor } from "./components/StudentLookForTutor";
import { StudentHelp } from "./components/StudentHelp";
import { StudentAccount } from "./components/StudentAccount";

import { TutorAppointments } from "./components/TutorAppointments";
import { TutorMessageRequests } from "./components/TutorMessageRequests";
import { TutorAccount } from "./components/TutorAccount";

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
        index: true,
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
    }

];

export default AppRoutes;
