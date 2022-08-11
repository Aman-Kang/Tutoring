import { Counter } from "./components/Counter";
import { FetchData } from "./components/FetchData";
import { StudentAppointments } from "./components/StudentAppointments";
import { StudentLookForTutor } from "./components/StudentLookForTutor";
import { StudentHelp } from "./components/StudentHelp";
import { StudentAccount } from "./components/StudentAccount";

import { TutorAppointments } from "./components/TutorAppointments";
import { TutorMessageRequests } from "./components/TutorMessageRequests";

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
        path: '/help',
        element: <StudentHelp />
    },
    {
        path: '/account',
        element: <StudentAccount />
    },
    {
        path: '/tutor-appointments',
        element: <TutorAppointments />
    },
    {
        path: '/tutor-message-requests',
        element: <TutorMessageRequests />
    }

];

export default AppRoutes;
