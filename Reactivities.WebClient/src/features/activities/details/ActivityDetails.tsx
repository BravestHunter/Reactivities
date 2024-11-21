import { useEffect } from 'react'
import { Grid, GridColumn } from 'semantic-ui-react'
import LoadingComponent from '../../../app/layout/LoadingComponent'
import { observer } from 'mobx-react-lite'
import { useParams } from 'react-router-dom'
import ActivityDetailedHeader from './ActivityDetailedHeader'
import ActivityDetailedInfo from './ActivityDetailedInfo'
import ActivityDetailedChat from './ActivityDetailedChat'
import ActivityDetailedSidebar from './ActivityDetailedSidebar'
import { useActivityStore } from '../../../app/stores/activityStore'

export default observer(function ActivityDetails() {
  const activityStore = useActivityStore()
  const {
    selectedActivity: activity,
    loadActivity,
    loading,
    clearSelectedActivity,
  } = activityStore
  const params = useParams<{ id: string }>()
  const id = Number(params.id)

  useEffect(() => {
    if (id) loadActivity(id)
    return () => clearSelectedActivity()
  }, [id, loadActivity, clearSelectedActivity])

  if (loading || !activity) return <LoadingComponent />

  return (
    <Grid>
      <GridColumn width={10}>
        <ActivityDetailedHeader activity={activity} />
        <ActivityDetailedInfo activity={activity} />
        <ActivityDetailedChat activityId={activity.id} />
      </GridColumn>
      <GridColumn width={6}>
        <ActivityDetailedSidebar activity={activity} />
      </GridColumn>
    </Grid>
  )
})
