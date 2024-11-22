import { Header } from 'semantic-ui-react'
import { useMainStore } from '../../../app/stores/mainStore'
import { observer } from 'mobx-react-lite'
import ActivityListItem from './ActivityListItem'
import { Fragment } from 'react'

export default observer(function ActivityList() {
  const { activityStore } = useMainStore()
  const { groupedActivities } = activityStore

  return (
    <>
      {groupedActivities.map(([group, activities]) => (
        <Fragment key={group}>
          <Header sub color="teal">
            {group}
          </Header>
          {activities.map((activity) => (
            <ActivityListItem key={activity.id} activity={activity} />
          ))}
        </Fragment>
      ))}
    </>
  )
})
