import { Header } from 'semantic-ui-react'
import { observer } from 'mobx-react-lite'
import ActivityListItem from './ActivityListItem'
import { Fragment } from 'react'
import { useActivityStore } from '../../../app/stores/activityStore'

export default observer(function ActivityList() {
  const activityStore = useActivityStore()
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
